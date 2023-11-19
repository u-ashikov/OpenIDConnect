namespace IdentityServer.Web.Data.Models;

public interface IConcurrencyAware
{
    string ConcurrencyStamp { get; set; }
}