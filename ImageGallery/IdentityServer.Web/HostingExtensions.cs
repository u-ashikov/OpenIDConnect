namespace IdentityServer.Web;

using IdentityServer.Web.Data.Models;
using Microsoft.AspNetCore.Identity;
using Data;
using Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<IISOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });
        
        builder.Services.Configure<IISServerOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });
        
        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

        builder.Services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("IdentityServerDb"));
        });

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddProfileService<LocalUserProfileService>()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<ILocalUserService, LocalUserService>();
        builder.Services.AddScoped<ILocalUserProfileService, LocalUserProfileService>();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseStaticFiles();
        app.UseRouting();
            
        app.UseIdentityServer();

        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
