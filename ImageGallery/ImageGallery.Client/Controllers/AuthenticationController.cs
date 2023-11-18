namespace ImageGallery.Client.Controllers;

using ImageGallery.Client.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AuthenticationController : Controller
{
    private readonly IdentityServerHttpClient _identityServerHttpClient;

    public AuthenticationController(IdentityServerHttpClient identityServerHttpClient)
    {
        this._identityServerHttpClient = identityServerHttpClient ?? throw new ArgumentNullException(nameof(identityServerHttpClient));
    }

    [Authorize]
    public async Task Logout(CancellationToken cancellationToken)
    {
        await this._identityServerHttpClient.RevokeAccessTokenAsync(cancellationToken).ConfigureAwait(false);
        
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        await this.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }

    public IActionResult AccessDenied()
        => this.View();
}