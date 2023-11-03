﻿namespace ImageGallery.API.Services;

using Entities;

public interface IGalleryRepository
{
    Task<IEnumerable<Image>> GetImagesAsync(string ownerId);
    Task<bool> IsImageOwnerAsync(Guid id, string ownerId);
    Task<Image?> GetImageAsync(Guid id, string ownerId);
    Task<bool> ImageExistsAsync(Guid id, string ownerId);
    void AddImage(Image image);
    void UpdateImage(Image image);
    void DeleteImage(Image image);
    Task<bool> SaveChangesAsync();
}