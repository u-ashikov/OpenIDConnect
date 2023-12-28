namespace IdentityServer.Web.Data.Models;

using System.ComponentModel.DataAnnotations;

public class UserSecret : IConcurrencyAware
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Secret { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public virtual User User { get; set; }
    
    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}