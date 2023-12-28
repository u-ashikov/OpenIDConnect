namespace IdentityServer.Web;

using IdentityServer.Web.Helpers;
using Duende.IdentityServer;
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

        builder.Services.AddAuthentication()
            .AddFacebook("Facebook", options =>
            {
                options.AppId = "1342182150003035";
                options.AppSecret = "bbaa5c12dbda4dd9edab087a94a1c4be";
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
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

        builder.Services
            .AddAuthentication()
            .AddOpenIdConnect("AAD","Azure Active Directory", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.Authority = "https://login.microsoftonline.com/4502e197-d331-412f-91c2-bab9c0f5b711/v2.0";
                options.ClientId = "f826d3b9-7c6c-433b-b6ef-2deb2864aca9";
                options.ClientSecret = "jcN8Q~27lEfKUjKDb~tJe8ZF1XrN5rUtWLFaXc4-";
                options.ResponseType = "code";
                options.Scope.Add("email");
                options.Scope.Add("offline_access");
                options.CallbackPath = new PathString("/signin-aad/");
                options.SignedOutCallbackPath = new PathString("/signout-aad/");
                options.SaveTokens = true;
            });

        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<ILocalUserService, LocalUserService>();
        builder.Services.AddScoped<ILocalUserProfileService, LocalUserProfileService>();
        builder.Services.AddSingleton<ExternalIdentityProviderClaimMapper>();

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
