namespace IdentityServer.Web.Services;

using IdentityServer.Web.Data.Models;

public class LocalUserService : ILocalUserService
{
    public Task<bool> ValidateCredentialsAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserBySubjectAsync(string subject)
    {
        throw new NotImplementedException();
    }

    public void AddUser(User userToAdd)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserActive(string subject)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}