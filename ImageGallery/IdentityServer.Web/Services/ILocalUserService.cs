﻿using System.Security.Claims;

namespace IdentityServer.Web.Services;

using IdentityServer.Web.Data.Models;

public interface ILocalUserService
{
    Task<bool> ValidateCredentialsAsync(string userName, string password, CancellationToken cancellationToken);

    Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject, CancellationToken cancellationToken);

    Task<User> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);

    Task<User> GetUserBySubjectAsync(string subject, CancellationToken cancellationToken);

    Task AddUser(User userToAdd, CancellationToken cancellationToken);

    Task<bool> IsUserActive(string subject, CancellationToken cancellationToken);

    Task<bool> ActivateUserAsync(string userName, string activationCode, CancellationToken cancellationToken);
    
    Task<User> EnsureExternalUserAsync(ClaimsPrincipal externalUser, string provider, string providerIdentityKey, CancellationToken cancellationToken);

    Task<bool> AddUserSecret(string subject, string name, string secret, CancellationToken cancellationToken);

    Task<UserSecret> GetUserSecretAsync(string subject, string name, CancellationToken cancellationToken);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}