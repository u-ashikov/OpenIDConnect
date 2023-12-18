using System.IdentityModel.Tokens.Jwt;
using ImageGallery.API.Authorization;
using ImageGallery.API.DbContexts;
using ImageGallery.API.Services;
using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddDbContext<GalleryContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ImageGalleryDBConnectionString"]);
});

// register the repository
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<IAuthorizationHandler, IsImageOwnerAuthorizationHandler>();

// register AutoMapper-related services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddOAuth2Introspection(options =>
    {
        options.Authority = "https://localhost:44300";
        options.ClientId = "imagegalleryapi";
        options.ClientSecret = "secret";
        options.NameClaimType = "given_name";
        options.RoleClaimType = "role";
    });
    // .AddJwtBearer(options =>
    // {
    //     options.Authority = "https://localhost:5001";
    //     options.Audience = "imagegalleryapi";
    //     options.TokenValidationParameters = new TokenValidationParameters()
    //     {
    //         NameClaimType = "given_name",
    //         RoleClaimType = "role",
    //         ValidTypes = new[] { "at+jwt" }
    //     };
    // });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ImageCreatorPolicy", AuthorizationPolicies.ImageCreatorPolicy());
    options.AddPolicy("ClientApplicationCanWrite", authorizationPolicy =>
    {
        authorizationPolicy.RequireClaim("scope", "imagegalleryapi.write");
    });
    options.AddPolicy("IsImageOwner", authorizationPolicy =>
    {
        authorizationPolicy.RequireAuthenticatedUser();
        authorizationPolicy.AddRequirements(new IsImageOwnerAuthorizationRequirement());
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
