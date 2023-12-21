using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Web.Data.Models;

public class UserLogin : IConcurrencyAware
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public virtual User User { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Provider { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string ProviderIdentityKey { get; set; }
    
    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}