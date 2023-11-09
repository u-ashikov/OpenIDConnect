using System.IdentityModel.Tokens.Jwt;
using ImageGallery.Authorization;
using ImageGallery.Client.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(configure => 
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.AccessDeniedPath = "/Authentication/AccessDenied";
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ClientId = "imagegallery";
        options.ClientSecret = "secret";
        options.Authority = "https://localhost:5001";
        options.ResponseType = "code";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("roles");
        options.Scope.Add("country");
        options.Scope.Add("imagegalleryapi.read");
        options.Scope.Add("imagegalleryapi.write");
        // This means receiving refresh tokens by the middleware.
        options.Scope.Add("offline_access");
        options.CallbackPath = "/signin-oidc";
        options.SignedOutCallbackPath = "/signout-callback-oidc";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClaimActions.DeleteClaim("amr");
        options.ClaimActions.DeleteClaim("sid");
        options.ClaimActions.MapJsonKey("role", "role");
        options.ClaimActions.MapJsonKey("country", "country");
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "given_name",
            RoleClaimType = "role",
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ImageCreatorPolicy", AuthorizationPolicies.ImageCreatorPolicy());
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BearerAuthenticationHandler>();

builder.Services.AddHttpClient<ImageGalleryApiHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ImageGalleryAPIRoot"]);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddHttpMessageHandler<BearerAuthenticationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();
