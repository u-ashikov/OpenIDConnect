namespace IdentityServer.Web.Pages.Account.Register;

using System.Security.Cryptography;
using IdentityModel;
using Data.Models;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[SecurityHeaders]
[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly ILocalUserService _localUserService;

    public IndexModel(ILocalUserService localUserService)
    {
        this._localUserService = localUserService ?? throw new ArgumentNullException(nameof(localUserService));
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

        var userEntity = new User()
        {
            UserName = this.InputModel.UserName,
            Password = this.InputModel.Password,
            Subject = this.InputModel.UserName,
            Email = this.InputModel.Email,
            ActivationCode = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128)),
            ActivationCodeExpirationDate = DateTime.UtcNow.AddHours(1),
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
        
        return this.RedirectToPage("/Account/RegisterSuccess/Index", new { registeredUser = userEntity.UserName, activationCode = userEntity.ActivationCode, returnUrl });
    }

    private void PrepareInputModel(string returnUrl)
    {
        this.InputModel = new RegisterInputModel()
        {
            ReturnUrl = returnUrl
        };
    }
}