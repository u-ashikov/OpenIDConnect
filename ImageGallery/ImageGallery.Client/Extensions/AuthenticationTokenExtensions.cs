namespace ImageGallery.Client.Extensions;

using Microsoft.AspNetCore.Authentication;

public static class AuthenticationTokenExtensions
{
    public static AuthenticationToken? GetByName(this IEnumerable<AuthenticationToken> authenticationTokens, string name)
    {
        var iteratedAuthenticationTokens = authenticationTokens.ToArray();

        return iteratedAuthenticationTokens.FirstOrDefault(t => t.Name == name);
    }
}