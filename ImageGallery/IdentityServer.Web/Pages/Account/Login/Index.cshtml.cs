using System.Text;
using OtpSharp;
using TwoStepsAuthenticator;

namespace IdentityServer.Web.Pages.Account.Login;

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Services;
using IdentityServer.Web.Pages.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IIdentityProviderStore _identityProviderStore;
    private readonly ILocalUserService _localUserService;

    public ViewModel View { get; set; }
        
    [BindProperty]
    public InputModel Input { get; set; }
        
    public Index(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        ILocalUserService localUserService)
    {
        this._interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
        this._schemeProvider = schemeProvider ?? throw new ArgumentNullException(nameof(schemeProvider));
        this._identityProviderStore = identityProviderStore ?? throw new ArgumentNullException(nameof(identityProviderStore));
        this._events = events ?? throw new ArgumentNullException(nameof(events));
        this._localUserService = localUserService ?? throw new ArgumentNullException(nameof(localUserService));
    }

    public async Task<IActionResult> OnGet(string returnUrl)
    {
        await BuildModelAsync(returnUrl);
            
        if (View.IsExternalLoginOnly)
        {
            // we only have one option for logging in and it's an external provider
            return RedirectToPage("/ExternalLogin/Challenge", new { scheme = View.ExternalLoginScheme, returnUrl });
        }

        return Page();
    }
        
    public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "login")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl);
            }
            
            // since we don't have a valid context, then we just go back to the home page
            return Redirect("~/");
        }

        if (ModelState.IsValid)
        {
            var validCredentials = await this._localUserService.ValidateCredentialsAsync(Input.Username, Input.Password, cancellationToken).ConfigureAwait(false);
            if (!validCredentials)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(Input.Username, "invalid credentials", clientId:context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
            }
            else
            {
                var existingUser = await this._localUserService.GetUserByUserNameAsync(Input.Username, cancellationToken).ConfigureAwait(false);
                var userSecret = await this._localUserService.GetUserSecretAsync(existingUser.Subject, "TOTP", cancellationToken).ConfigureAwait(false);
                if (userSecret is null)
                {
                    this.ModelState.AddModelError(string.Empty, "No second factor secret configured. Please reach out to your administrator.");
                    await this.BuildModelAsync(Input.ReturnUrl).ConfigureAwait(false);
                    return this.Page();
                }

                var authenticator = new TimeAuthenticator();
                if (!authenticator.CheckCode(userSecret.Secret, Input.Totp, existingUser))
                {
                    this.ModelState.AddModelError(string.Empty, "TOTP is not valid.");
                    await this.BuildModelAsync(Input.ReturnUrl).ConfigureAwait(false);
                    return this.Page();
                }
                
                await _events.RaiseAsync(new UserLoginSuccessEvent(existingUser.UserName, existingUser.Subject, existingUser.UserName, clientId: context?.Client.ClientId));

                // only set explicit expiration here if user chooses "remember me". 
                // otherwise we rely upon expiration configured in cookie middleware.
                AuthenticationProperties props = null;
                if (LoginOptions.AllowRememberLogin && Input.RememberLogin)
                {
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(LoginOptions.RememberMeLoginDuration)
                    };
                };

                // issue authentication cookie with subject ID and username
                var isuser = new IdentityServerUser(existingUser.Subject)
                {
                    DisplayName = existingUser.UserName
                };

                await HttpContext.SignInAsync(isuser, props);

                if (context != null)
                {
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage(Input.ReturnUrl);
                    }

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(Input.ReturnUrl);
                }

                // request for a local page
                if (Url.IsLocalUrl(Input.ReturnUrl))
                    return Redirect(Input.ReturnUrl);
                
                if (string.IsNullOrEmpty(Input.ReturnUrl))
                    return Redirect("~/");
                
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }
        }

        // something went wrong, show form with error
        await BuildModelAsync(Input.ReturnUrl);
        return Page();
    }
        
    private async Task BuildModelAsync(string returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl
        };
            
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            View = new ViewModel
            {
                EnableLocalLogin = local,
            };

            Input.Username = context?.LoginHint;

            if (!local)
            {
                View.ExternalProviders = new[] { new ViewModel.ExternalProvider { AuthenticationScheme = context.IdP } };
            }

            return;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ViewModel.ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var dyanmicSchemes = (await _identityProviderStore.GetAllSchemeNamesAsync())
            .Where(x => x.Enabled)
            .Select(x => new ViewModel.ExternalProvider
            {
                AuthenticationScheme = x.Scheme,
                DisplayName = x.DisplayName
            });
        providers.AddRange(dyanmicSchemes);


        var allowLocal = true;
        var client = context?.Client;
        if (client != null)
        {
            allowLocal = client.EnableLocalLogin;
            if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
            {
                providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
            }
        }

        View = new ViewModel
        {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
            ExternalProviders = providers.ToArray()
        };
    }
}