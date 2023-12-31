﻿namespace ImageGallery.Client.Infrastructure;

using System.Text.Json;
using ImageGallery.Client.Common;
using ImageGallery.Model;

public class ImageGalleryApiHttpClient
{
    private readonly HttpClient _httpClient;

    public ImageGalleryApiHttpClient(HttpClient httpClient)
    {
        this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, WebConstants.ApiClient.GetAllImagesEndpoint);
        var response = await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var images = await JsonSerializer.DeserializeAsync<IEnumerable<Image>>(responseContent, cancellationToken: cancellationToken).ConfigureAwait(false);

        return images;
    }

    public async Task<Image> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = string.Format(WebConstants.ApiClient.ImageByIdEndpoint, id);
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        
        var response = await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var foundImage = await JsonSerializer.DeserializeAsync<Image>(responseContent, cancellationToken: cancellationToken).ConfigureAwait(false);

        return foundImage;
    }

    public async Task CreateAsync(ImageForCreation imageForCreation, CancellationToken cancellationToken)
    {
        var serializedImageForCreation = JsonSerializer.Serialize(imageForCreation);

        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            WebConstants.ApiClient.GetAllImagesEndpoint)
        {
            Content = new StringContent(
                serializedImageForCreation,
                System.Text.Encoding.Unicode,
                "application/json")
        };

        var response = await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(string id, ImageForUpdate imageForUpdate, CancellationToken cancellationToken)
    {
        var serializedImageForUpdate = JsonSerializer.Serialize(imageForUpdate);
        var url = string.Format(WebConstants.ApiClient.ImageByIdEndpoint, id);
        
        var httpRequest = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = new StringContent(
                serializedImageForUpdate,
                System.Text.Encoding.Unicode,
                "application/json")
        };

        var response = await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var url = string.Format(WebConstants.ApiClient.ImageByIdEndpoint, id);
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);

        var response = await this._httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}