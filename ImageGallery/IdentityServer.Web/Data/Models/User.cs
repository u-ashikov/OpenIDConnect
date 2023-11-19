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

    [Required]
    public bool Active { get; set; }

    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
}