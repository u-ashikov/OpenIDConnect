namespace IdentityServer.Web.Data.Models;

using System.ComponentModel.DataAnnotations;

public class User : IConcurrencyAware
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(200)]
    [Required]
    public string Subject { get; set; }

    [MaxLength(200)]
    public string UserName { get; set; }

    [MaxLength(200)]
    public string Password { get; set; }
    
    [MaxLength(200)]
    [Required]
    public string Email { get; set; }

    [Required]
    public bool Active { get; set; }
    
    [MaxLength(200)]
    public string ActivationCode { get; set; }
    
    public DateTime ActivationCodeExpirationDate { get; set; }

    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

    public virtual ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
}