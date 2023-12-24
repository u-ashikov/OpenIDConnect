using System.Net;

namespace IdentityServer.Web.Pages.MfaRegistration;

using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[SecurityHeaders]
[Authorize]
public class IndexModel : PageModel
{
    private readonly char[] _alphaNumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
    
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

        this.ViewModel = new RegisterMfaSecretViewModel()
        {
            KeyUri = string.Format("otpauth://totp/{0}:{1}?secret={2}&issuer={0}", WebUtility.UrlEncode("IDP"),
                WebUtility.UrlEncode(existingUser.Email), this.GenerateSecret()),
        };

        this.InputModel = new RegisterMfaSecretInputModel()
        {
            Secret = this.GenerateSecret(),
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
            return this.RedirectToPage("~/");

        await this._localUserService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        this.ModelState.AddModelError(string.Empty, "Unable to add the secret you have provided for MFA.");
        return this.Page();
    }

    private string GenerateSecret()
    {
        var tokenData = RandomNumberGenerator.GetBytes(64);

        var result = new StringBuilder(16);
        for (var i = 0; i < 16; i++)
        {
            var rnd = BitConverter.ToUInt32(tokenData, i * 4);
            var idx = rnd % this._alphaNumericCharacters.Length;

            result.Append(this._alphaNumericCharacters[idx]);
        }

        return result.ToString();
    }
}