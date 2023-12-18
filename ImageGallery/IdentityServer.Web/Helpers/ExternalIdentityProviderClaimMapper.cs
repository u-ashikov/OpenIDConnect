namespace IdentityServer.Web.Helpers;

using System.Security.Claims;
using IdentityModel;

public class ExternalIdentityProviderClaimMapper
{
    private readonly IDictionary<string, Func<string, string>> _externalIdentityProviderClaimMappings = new Dictionary<string, Func<string, string>>();

    public ExternalIdentityProviderClaimMapper()
    {
        this._externalIdentityProviderClaimMappings.Add("Facebook", this.MapFacebookClaim);
    }

    public ICollection<Claim> MapClaims(string authenticationScheme, IEnumerable<Claim> inputClaims)
    {
        if (inputClaims is null)
            return null;

        if (!this._externalIdentityProviderClaimMappings.ContainsKey(authenticationScheme))
            return null;

        var claimMapper = this._externalIdentityProviderClaimMappings[authenticationScheme];
        var iteratedInputClaims = inputClaims.ToArray();
        var outputClaims = new List<Claim>(capacity: iteratedInputClaims.Length);

        foreach (var claim in iteratedInputClaims)
        {
            var outputClaimType = claimMapper(claim.Type);
            if (string.IsNullOrWhiteSpace(outputClaimType)) continue;
            
            outputClaims.Add(new Claim(outputClaimType, claim.Value));
        }

        return outputClaims;
    }

    private string MapFacebookClaim(string inputClaim)
    {
        return inputClaim switch
        {
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname" => JwtClaimTypes.GivenName,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname" => JwtClaimTypes.FamilyName,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" => JwtClaimTypes.Email,
            _ => string.Empty
        };
    }
}