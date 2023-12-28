namespace IdentityServer.Web.Pages.MfaRegistration;

using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[SecurityHeaders]
[Authorize]
public class IndexModel : PageModel
{
    private readonly ILocalUserService _localUserService;

    public IndexModel(ILocalUserService localUserService)
    {
        this._localUserService = localUserService ?? throw new ArgumentNullException(nameof(localUserService));
    }
    
    public RegisterMfaSecretViewModel ViewModel { get; set; }
    
    [BindProperty]
    public RegisterMfaSecretInputModel InputModel { get; set; }

    public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        var userSubject = this.User.FindFirst(JwtClaimTypes.Subject)?.Value;
        var existingUser = await this._localUserService.GetUserBySubjectAsync(userSubject, cancellationToken).ConfigureAwait(false);
        if (existingUser is null)
            return this.Unauthorized();
        
        var secret = TwoStepsAuthenticator.Authenticator.GenerateKey();

        this.ViewModel = new RegisterMfaSecretViewModel()
        {
            KeyUri = string.Format("otpauth://totp/{0}:{1}?secret={2}&issuer={0}", WebUtility.UrlEncode("IDP"),
                WebUtility.UrlEncode(existingUser.Email), secret),
        };

        this.InputModel = new RegisterMfaSecretInputModel()
        {
            Secret = secret,
        };
        
        return this.Page();
    }

    public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
    {
        if (!this.ModelState.IsValid)
            return this.Page();
        
        var userSubject = this.User.FindFirst(JwtClaimTypes.Subject)?.Value;
        var addUserSecret = await this._localUserService.AddUserSecret(userSubject, "TOTP", this.InputModel.Secret, cancellationToken).ConfigureAwait(false);

        if (addUserSecret)
        {
            await this._localUserService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return this.RedirectToPage("~/");
        }

        await this._localUserService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        this.ModelState.AddModelError(string.Empty, "Unable to add the secret you have provided for MFA.");
        return this.Page();
    }
    
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        context.HttpContext.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; img-src 'self' data:; script-src 'self'");

        base.OnPageHandlerExecuting(context);
    }
}