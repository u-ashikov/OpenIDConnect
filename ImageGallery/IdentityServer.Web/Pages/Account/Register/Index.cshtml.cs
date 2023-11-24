namespace IdentityServer.Web.Pages.Account.Register;

using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityServer.Web.Data.Models;
using IdentityServer.Web.Services;
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

    [BindProperty]
    public RegisterInputModel InputModel { get; set; }
    
    public IActionResult OnGet(string returnUrl)
    {
        this.PrepareInputModel(returnUrl);

        return Page();
    }

    public async Task<IActionResult> OnPost(string returnUrl, CancellationToken cancellationToken)
    {
        if (!this.ModelState.IsValid)
            return this.Page();

        var userEntity = new Data.Models.User()
        {
            UserName = this.InputModel.UserName,
            Password = this.InputModel.Password,
            Active = true,
        };
        
        userEntity.Claims.Add(new UserClaim()
        {
            Type = JwtClaimTypes.GivenName,
            Value = this.InputModel.GivenName,
        });
        
        userEntity.Claims.Add(new UserClaim()
        {
            Type = JwtClaimTypes.FamilyName,
            Value = this.InputModel.FamilyName,
        });
        
        userEntity.Claims.Add(new UserClaim()
        {
            Type = "country",
            Value = this.InputModel.Country,
        });

        await this._localUserService.AddUser(userEntity, cancellationToken).ConfigureAwait(false);
        await this._localUserService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await this.HttpContext.SignInAsync(new IdentityServerUser(this.InputModel.UserName)).ConfigureAwait(false);

        if (this._identityServerInteractionService.IsValidReturnUrl(this.InputModel.ReturnUrl))
            return this.Redirect(this.InputModel.ReturnUrl);

        return this.Page();
    }

    private void PrepareInputModel(string returnUrl)
    {
        this.InputModel = new RegisterInputModel()
        {
            ReturnUrl = returnUrl
        };
    }
}