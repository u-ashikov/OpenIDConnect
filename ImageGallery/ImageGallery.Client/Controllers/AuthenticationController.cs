using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace ImageGallery.Client.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AuthenticationController : Controller
{
    [Authorize]
    public async Task Logout(CancellationToken cancellationToken)
    {
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        await this.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
}