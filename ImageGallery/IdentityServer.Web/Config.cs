using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer.Web;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client()
            {
                ClientId = "imagegallery",
                ClientSecrets = new List<Secret>()
                {
                    new Secret("secret".ToSha256())
                },
                ClientName = "Image Gallery",
                RedirectUris = new List<string>()
                {
                    "https://localhost:5001/signin-oidc"
                },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                }
            }
        };
}