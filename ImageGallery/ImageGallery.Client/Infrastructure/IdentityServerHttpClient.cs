namespace ImageGallery.Client.Infrastructure;

using Microsoft.AspNetCore.Authentication;

public class IdentityServerHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityServerHttpClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task RevokeAccessTokenAsync(CancellationToken cancellationToken)
    {
        var accessToken = await this._httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);
        
        var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/connect/revocation");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("token", accessToken),
            new KeyValuePair<string, string>("client_id", "imagegallery"),
            new KeyValuePair<string, string>("client_secret", "secret"),
            new KeyValuePair<string, string>("token_type_hint", "access_token"),
        });

        var result = await this._httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        result.EnsureSuccessStatusCode();
    }
}