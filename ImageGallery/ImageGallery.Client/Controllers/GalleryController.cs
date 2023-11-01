namespace ImageGallery.Client.Controllers;

using Common;
using Infrastructure;
using ViewModels;
using Model;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class GalleryController : Controller
{
    private readonly ImageGalleryApiHttpClient _imageGalleryApiHttpClient;
    private readonly ILogger<GalleryController> _logger;

    public GalleryController(ImageGalleryApiHttpClient imageGalleryApiHttpClient, ILogger<GalleryController> logger)
    {
        this._imageGalleryApiHttpClient = imageGalleryApiHttpClient ?? throw new ArgumentNullException(nameof(imageGalleryApiHttpClient));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        await this.LogUserInformationAsync(cancellationToken).ConfigureAwait(false);

        var images = await this._imageGalleryApiHttpClient.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return View(new GalleryIndexViewModel(images));
    }

    public async Task<IActionResult> EditImage(Guid id, CancellationToken cancellationToken)
    {
        var image = await this._imageGalleryApiHttpClient.GetByIdAsync(id.ToString(), cancellationToken).ConfigureAwait(false);

        if (image == null)
            throw new Exception(WebConstants.Messages.InvalidImage);

        var editImageViewModel = new EditImageViewModel()
        {
            Id = image.Id,
            Title = image.Title
        };

        return this.View(editImageViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditImage(EditImageViewModel editImageViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return this.View();
        
        var imageForUpdate = new ImageForUpdate(editImageViewModel.Title);

        // TODO: Return operation result.
        await this._imageGalleryApiHttpClient.UpdateAsync(editImageViewModel.Id.ToString(), imageForUpdate, cancellationToken).ConfigureAwait(false);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteImage(Guid id, CancellationToken cancellationToken)
    {
        await this._imageGalleryApiHttpClient.DeleteAsync(id.ToString(), cancellationToken).ConfigureAwait(false);

        return this.RedirectToAction("Index");
    }

    [Authorize(Roles = "ProUser")]
    public IActionResult AddImage() => this.View();

    [Authorize(Roles = "ProUser")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddImage(AddImageViewModel addImageViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View();

        // create an ImageForCreation instance
        ImageForCreation? imageForCreation = null;

        // take the first (only) file in the Files list
        var imageFile = addImageViewModel.Files.First();

        if (imageFile.Length > 0)
        {
            await using var fileStream = imageFile.OpenReadStream();
            using var ms = new MemoryStream();
            
            fileStream.CopyTo(ms);
            imageForCreation = new ImageForCreation(addImageViewModel.Title, ms.ToArray());
        }

        await this._imageGalleryApiHttpClient.CreateAsync(imageForCreation, cancellationToken).ConfigureAwait(false);

        return RedirectToAction("Index");
    }

    private async Task LogUserInformationAsync(CancellationToken cancellationToken)
    {
        var idToken = await this.HttpContext.GetTokenAsync(OpenIdConnectDefaults.AuthenticationScheme, "id_token").ConfigureAwait(false);
        var accessToken = await this.HttpContext.GetTokenAsync(OpenIdConnectDefaults.AuthenticationScheme, "access_token").ConfigureAwait(false);
            
        this._logger.LogInformation($"Identity Token: {idToken}");
        this._logger.LogInformation($"Access Token: {accessToken}");

        foreach (var userClaim in this.User.Claims)
            this._logger.LogInformation($"ClaimType: {userClaim.Type}, ClaimValue: {userClaim.Value}.");
    }
}