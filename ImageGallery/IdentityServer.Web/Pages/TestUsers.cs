// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Web.Pages;

using System.Security.Claims;
using Duende.IdentityServer.Test;
using IdentityModel;

public class TestUsers
{
    public static List<TestUser> Users =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                Username = "David",
                Password = "password",

                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "David"),
                    new Claim(JwtClaimTypes.FamilyName, "Flagg"),
                    new Claim(JwtClaimTypes.Role, "FreeUser"),
                    new Claim("country", "usa"),
                }
            },
            new TestUser
            {
                SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                Username = "Emma",
                Password = "password",

                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "Emma"),
                    new Claim(JwtClaimTypes.FamilyName, "Flagg"),
                    new Claim(JwtClaimTypes.Role, "ProUser"),
                    new Claim("country", "bel"),
                }
            }
        };
}