﻿namespace IdentityServer.Web.Data.Models;

using System.ComponentModel.DataAnnotations;

public class UserClaim : IConcurrencyAware
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(250)]
    [Required]
    public string Type { get; set; }

    [MaxLength(250)]
    [Required]
    public string Value { get; set; }

    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public Guid UserId { get; set; }

    public virtual User User { get; set; }
}