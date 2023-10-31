namespace ImageGallery.Client.Infrastructure;

using System.Net.Http.Headers;
using System.Text.Json;
using ImageGallery.Client.Common;
using ImageGallery.Model;
using Microsoft.AspNetCore.Authentication;

public class ImageGalleryApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageGalleryApiHttpClient(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
    {
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken)
    {
        var response = await this.SendInternallyAsync(WebConstants.ApiClient.GetAllImagesEndpoint, HttpMethod.Get, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var images = await JsonSerializer.DeserializeAsync<IEnumerable<Image>>(responseContent, cancellationToken: cancellationToken).ConfigureAwait(false);

        return images;
    }

    private async Task<HttpResponseMessage> SendInternallyAsync(string url, HttpMethod httpMethod, CancellationToken cancellationToken)
    {
        var accessToken = await this._httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);
        var httpRequest = new HttpRequestMessage(httpMethod, url);
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
    }
}