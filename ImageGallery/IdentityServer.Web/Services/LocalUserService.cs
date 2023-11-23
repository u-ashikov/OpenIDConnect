namespace IdentityServer.Web.Services;

using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

public class LocalUserService : ILocalUserService
{
    private readonly IdentityDbContext _identityDbContext;

    public LocalUserService(IdentityDbContext identityDbContext)
    {
        this._identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
    }

    public Task<bool> ValidateCredentialsAsync(string userName, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            return Task.FromResult(false);

        return this._identityDbContext.Users.AnyAsync(u => u.UserName == userName && u.Password == password, cancellationToken);
    }

    public async Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var existingUser = await this._identityDbContext.Users.Include(u => u.Claims).FirstOrDefaultAsync(u => u.Subject == subject, cancellationToken).ConfigureAwait(false);

        if (existingUser is null)
            return Enumerable.Empty<UserClaim>();

        return existingUser.Claims;
    }

    public Task<User> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return Task.FromResult((User)null);

        return this._identityDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public Task<User> GetUserBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(subject))
            return Task.FromResult((User)null);

        return this._identityDbContext.Users.FirstOrDefaultAsync(u => u.Subject == subject, cancellationToken);
    }

    public void AddUser(User userToAdd)
    {
        if (userToAdd is null)
            return;

        this._identityDbContext.Users.Add(userToAdd);
    }

    public async Task<bool> IsUserActive(string subject, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(subject))
            return false;

        var existingUser = await this.GetUserBySubjectAsync(subject, cancellationToken).ConfigureAwait(false);
        return existingUser is not null && existingUser.Active;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var saveChanges = await this._identityDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return saveChanges > 0;
    }
}