﻿namespace IdentityServer.Web;

using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "Your roles", new[] { "role" }),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("imagegalleryapi", "Image Gallery API", new[] { "role" })
            {
                Scopes = { "imagegalleryapi.fullaccess" }
            },
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("imagegalleryapi.fullaccess")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client()
            {
                ClientId = "imagegallery",
                ClientName = "Image Gallery",
                ClientSecrets = new List<Secret>()
                {
                    new Secret("secret".ToSha256())
                },
                RedirectUris =
                {
                    "https://localhost:7184/signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:7184/signout-callback-oidc"
                },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles",
                    "imagegalleryapi.fullaccess",
                }
            }
        };
}