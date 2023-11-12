using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ImageGallery.Client.Infrastructure;

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
        var authenticationResult = await this._httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
        var allTokens = authenticationResult.Properties.GetTokens();

        var accessTokenExpiresAtToken = allTokens.FirstOrDefault(t => t.Name == "expires_at");
        var accessTokenExpiresAt = DateTime.Parse(accessTokenExpiresAtToken.Value);
        
        var accessToken = await this._httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);

        var dateDiff = (accessTokenExpiresAt.ToUniversalTime() - DateTime.UtcNow).TotalSeconds;
        if (dateDiff <= 60)
        {
            var refreshToken = allTokens.FirstOrDefault(t => t.Name == "refresh_token");
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

            var authenticationProperties = new List<AuthenticationToken>(capacity: 4);
            authenticationProperties.Add(new AuthenticationToken() { Name = OpenIdConnectParameterNames.IdToken, Value = refreshTokenResponse.IdToken });
            authenticationProperties.Add(new AuthenticationToken() { Name = OpenIdConnectParameterNames.AccessToken, Value = refreshTokenResponse.AccessToken });
            authenticationProperties.Add(new AuthenticationToken() { Name = OpenIdConnectParameterNames.RefreshToken, Value = refreshTokenResponse.RefreshToken });
            authenticationProperties.Add(new AuthenticationToken() { Name = "expires_at", Value = (DateTime.UtcNow + TimeSpan.FromSeconds(refreshTokenResponse.ExpiresIn)).
                ToString("o", CultureInfo.InvariantCulture) });
            
            authenticationResult.Properties.StoreTokens(authenticationProperties);

            await this._httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                authenticationResult.Principal,
                authenticationResult.Properties)
                .ConfigureAwait(false);
        }
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // return await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}

public class RefreshTokenResponse
{
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }
    
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}