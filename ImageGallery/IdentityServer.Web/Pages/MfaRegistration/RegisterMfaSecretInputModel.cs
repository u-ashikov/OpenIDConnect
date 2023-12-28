namespace IdentityServer.Web.Pages.MfaRegistration;

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public class RegisterMfaSecretInputModel
{
    [Required]
    [HiddenInput]
    public string Secret { get; set; }
}