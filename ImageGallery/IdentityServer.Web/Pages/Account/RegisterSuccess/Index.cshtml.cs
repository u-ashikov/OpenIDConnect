namespace IdentityServer.Web.Pages.Account.RegisterSuccess;

using IdentityServer.Web.Services;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[SecurityHeaders]
[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly ILocalUserService _localUserService;
    private readonly IIdentityServerInteractionService _identityServerInteractionService;

    public IndexModel(ILocalUserService localUserService, IIdentityServerInteractionService identityServerInteractionService)
    {
        this._localUserService = localUserService ?? throw new ArgumentNullException(nameof(localUserService));
        this._identityServerInteractionService = identityServerInteractionService ?? throw new ArgumentNullException(nameof(identityServerInteractionService));
    }
    
    [BindProperty(SupportsGet = true)]
    public string RegisteredUser { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string ActivationCode { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    public IActionResult OnGet()
    {
        return this.Page();
    }

    public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
    {
        var activateAccount = await this._localUserService.ActivateUserAsync(this.RegisteredUser, this.ActivationCode, cancellationToken).ConfigureAwait(false);
        if (activateAccount && this._identityServerInteractionService.IsValidReturnUrl(this.ReturnUrl))
            return this.Redirect(this.ReturnUrl);

        return this.Page();
    }
}