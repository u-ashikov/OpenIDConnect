namespace IdentityServer.Web.Services;

using IdentityServer.Web.Data.Models;

public interface ILocalUserService
{
    Task<bool> ValidateCredentialsAsync(string userName, string password, CancellationToken cancellationToken);

    Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject, CancellationToken cancellationToken);

    Task<User> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);

    Task<User> GetUserBySubjectAsync(string subject, CancellationToken cancellationToken);

    void AddUser(User userToAdd);

    Task<bool> IsUserActive(string subject, CancellationToken cancellationToken);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}