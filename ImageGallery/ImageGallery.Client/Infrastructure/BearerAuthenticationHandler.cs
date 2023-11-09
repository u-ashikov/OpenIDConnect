using System.Net.Http.Headers;

namespace ImageGallery.Client.Infrastructure;

using Microsoft.AspNetCore.Authentication;

public class BearerAuthenticationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BearerAuthenticationHandler(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // var authenticationResult = await this._httpContextAccessor.HttpContext.AuthenticateAsync().ConfigureAwait(false);
        
        var accessToken = await this._httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // return await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}