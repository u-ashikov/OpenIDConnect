namespace ImageGallery.Client.Infrastructure;

using ImageGallery.Client.Extensions;
using ImageGallery.Client.Infrastructure.Models;
using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

public class BearerAuthenticationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public BearerAuthenticationHandler(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await this.GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var authenticationResult = await this._httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
        var allTokens = authenticationResult.Properties.GetTokens().ToArray();
        var currentAccessToken =  await this._httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);

        if (!IsExpiring(allTokens))
            return currentAccessToken;
        
        var refreshToken = allTokens.GetByName("refresh_token");
        var identityServerHttpClient = this._httpClientFactory.CreateClient("IdentityServerHttpClient");
        var connectTokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/connect/token");

        var requestData = new[]
        {
            new KeyValuePair<string, string>("client_id", "imagegallery"),
            new KeyValuePair<string, string>("client_secret", "secret"),
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshToken.Value),
        };
        
        connectTokenRequest.Content = new FormUrlEncodedContent(requestData);
            
        var result = await identityServerHttpClient.SendAsync(connectTokenRequest, cancellationToken).ConfigureAwait(false);
        result.EnsureSuccessStatusCode();
        
        var refreshTokenResponse = await result.Content.ReadFromJsonAsync<RefreshTokenResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);

        var authenticationProperties = new List<AuthenticationToken>(capacity: 4)
        {
            new() { Name = OpenIdConnectParameterNames.IdToken, Value = refreshTokenResponse.IdToken },
            new() { Name = OpenIdConnectParameterNames.AccessToken, Value = refreshTokenResponse.AccessToken },
            new() { Name = OpenIdConnectParameterNames.RefreshToken, Value = refreshTokenResponse.RefreshToken },
            new() { Name = "expires_at", Value = (DateTime.UtcNow + TimeSpan.FromSeconds(refreshTokenResponse.ExpiresIn)).ToString("o", CultureInfo.InvariantCulture) }
        };

        authenticationResult.Properties.StoreTokens(authenticationProperties);

        await this._httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                authenticationResult.Principal,
                authenticationResult.Properties)
            .ConfigureAwait(false);

        return refreshTokenResponse.AccessToken;
    }

    private static bool IsExpiring(IEnumerable<AuthenticationToken> allTokens)
    {
        var accessTokenExpiresAtToken = allTokens.GetByName("expires_at");
        var accessTokenExpiresAt = DateTime.Parse(accessTokenExpiresAtToken.Value);

        var dateDiff = (accessTokenExpiresAt.ToUniversalTime() - DateTime.UtcNow).TotalSeconds;
        return dateDiff <= 60;
    }
}