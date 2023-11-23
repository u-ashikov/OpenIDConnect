namespace IdentityServer.Web.Services;

using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;

public class LocalUserProfileService : ILocalUserProfileService
{
    private readonly ILocalUserService _localUserService;

    public LocalUserProfileService(ILocalUserService localUserService)
    {
        this._localUserService = localUserService ?? throw new ArgumentNullException(nameof(localUserService));
    }

    /// <inheritdoc/>
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userSubject = context.Subject.GetSubjectId();
        var userClaims = await this._localUserService.GetUserClaimsBySubjectAsync(userSubject, CancellationToken.None).ConfigureAwait(false);
        
        context.AddRequestedClaims(userClaims.Select(c => new Claim(c.Type, c.Value)));
    }

    /// <inheritdoc/>
    public async Task IsActiveAsync(IsActiveContext context)
    {
        var userSubject = context.Subject.GetSubjectId();
        var userIsActive = await this._localUserService.IsUserActive(userSubject, CancellationToken.None).ConfigureAwait(false);

        context.IsActive = userIsActive;
    }
}