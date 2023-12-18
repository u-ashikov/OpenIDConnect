using System.Security.Claims;
using IdentityModel;

namespace IdentityServer.Web.Services;

using Microsoft.AspNetCore.Identity;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

public class LocalUserService : ILocalUserService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LocalUserService(IdentityDbContext identityDbContext, IPasswordHasher<User> passwordHasher)
    {
        this._identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
        this._passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<bool> ValidateCredentialsAsync(string userName, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            return false;

        var existingUser = await this._identityDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken).ConfigureAwait(false);
        if (existingUser is null)
            return false;
        
        var verifyHashedPassword = this._passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, password);
        return verifyHashedPassword == PasswordVerificationResult.Success;
    }

    public async Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var existingUser = await this._identityDbContext.Users.Include(u => u.Claims).FirstOrDefaultAsync(u => u.Subject == subject && u.Active, cancellationToken).ConfigureAwait(false);

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

        return this._identityDbContext.Users.FirstOrDefaultAsync(u => u.Subject == subject && u.Active, cancellationToken);
    }

    public async Task AddUser(User userToAdd, CancellationToken cancellationToken)
    {
        if (userToAdd is null)
            return;
        
        var userAlreadyExists = await this._identityDbContext.Users.AnyAsync(u => u.UserName == userToAdd.UserName, cancellationToken).ConfigureAwait(false);
        if (userAlreadyExists)
            throw new Exception("User with that username already exists.");

        userToAdd.Password = this._passwordHasher.HashPassword(userToAdd, userToAdd.Password);

        this._identityDbContext.Users.Add(userToAdd);
    }

    public async Task<bool> IsUserActive(string subject, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(subject))
            return false;

        var existingUser = await this.GetUserBySubjectAsync(subject, cancellationToken).ConfigureAwait(false);
        return existingUser is not null && existingUser.Active;
    }

    public async Task<bool> ActivateUserAsync(string userName, string activationCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(activationCode))
            return false;

        var existingUser = await this.GetUserByUserNameAsync(userName, cancellationToken).ConfigureAwait(false);
        if (existingUser is null)
            return false;

        if (DateTime.UtcNow > existingUser.ActivationCodeExpirationDate || existingUser.ActivationCode != activationCode)
            return false;

        existingUser.ActivationCode = null;
        existingUser.Active = true;

        await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    public async Task<User> FindByExternalProvider(string provider, string providerIdentityKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(provider))
            throw new InvalidOperationException();

        if (string.IsNullOrWhiteSpace(providerIdentityKey))
            throw new InvalidCastException();

        var existingUserLogin = await this._identityDbContext.UserLogins.Include(ul => ul.User)
            .FirstOrDefaultAsync(ul => ul.Provider == provider && ul.ProviderIdentityKey == providerIdentityKey, cancellationToken);

        return existingUserLogin?.User;
    }

    public User AutoProvisionUser(string provider, string providerIdentityKey, IEnumerable<Claim> claims)
    {
        if (string.IsNullOrWhiteSpace(provider))
            throw new InvalidOperationException();

        if (string.IsNullOrWhiteSpace(providerIdentityKey))
            throw new InvalidCastException();

        if (claims is null)
            throw new InvalidCastException();

        var user = new User()
        {
            Email = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email).Value,
            Active = true,
            Subject = Guid.NewGuid().ToString(),
        };

        foreach (var claim in claims)
        {
            user.Claims.Add(new UserClaim()
            {
                Type = claim.Type,
                Value = claim.Value,
            });
        }
        
        user.Logins.Add(new UserLogin()
        {
            Provider = provider,
            ProviderIdentityKey = providerIdentityKey,
        });

        this._identityDbContext.Users.Add(user);
        return user;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var saveChanges = await this._identityDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return saveChanges > 0;
    }
}