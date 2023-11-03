using System.Security.Claims;

namespace ImageGallery.API.Extensions;

public static class ModelsExtensions
{
    public static string? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal is null)
            return string.Empty;

        return claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    }
}