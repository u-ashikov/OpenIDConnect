namespace IdentityServer.Web.Services;

using Duende.IdentityServer.Models;

public class LocalUserProfileService : ILocalUserProfileService
{
    /// <inheritdoc/>
    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task IsActiveAsync(IsActiveContext context)
    {
        throw new NotImplementedException();
    }
}