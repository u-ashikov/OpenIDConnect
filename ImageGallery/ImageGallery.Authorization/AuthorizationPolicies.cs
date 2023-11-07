namespace ImageGallery.Authorization;

using Microsoft.AspNetCore.Authorization;

public static class AuthorizationPolicies
{
    public static AuthorizationPolicy ImageCreatorPolicy()
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("ProUser")
            .RequireClaim("country", "bel")
            .Build();

        return policy;
    }
}