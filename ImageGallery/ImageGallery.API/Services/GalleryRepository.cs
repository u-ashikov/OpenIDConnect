namespace ImageGallery.API.Services;

using DbContexts;
using Entities;
using Microsoft.EntityFrameworkCore;

public class GalleryRepository : IGalleryRepository 
{
    private readonly GalleryContext _context;

    public GalleryRepository(GalleryContext galleryContext)
    {
        this._context = galleryContext ?? throw new ArgumentNullException(nameof(galleryContext));
    }

    public Task<bool> ImageExistsAsync(Guid id, string ownerId)
        => this._context.Images.AnyAsync(i => i.Id == id && i.OwnerId == ownerId);

    public Task<Image?> GetImageAsync(Guid id, string ownerId)
        => this._context.Images.FirstOrDefaultAsync(i => i.Id == id && i.OwnerId == ownerId);
  
    public async Task<IEnumerable<Image>> GetImagesAsync(string ownerId)
        => await this._context.Images.Where(i => i.OwnerId == ownerId).OrderBy(i => i.Title).ToListAsync();

    public async Task<bool> IsImageOwnerAsync(Guid id, string ownerId)
        => await this._context.Images.AnyAsync(i => i.Id == id && i.OwnerId == ownerId);
        
    public void AddImage(Image image) => this._context.Images.Add(image);

    public void UpdateImage(Image image)
    {
        // no code in this implementation
    }

    public void DeleteImage(Image image)
    {
        this._context.Images.Remove(image);

        // Note: in a real-life scenario, the image itself potentially should 
        // be removed from disk.  We don't do this in this demo
        // scenario to allow for easier testing / re-running the code
    }

    public async Task<bool> SaveChangesAsync()
        => await this._context.SaveChangesAsync() >= 0;
}