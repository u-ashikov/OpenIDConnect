namespace ImageGallery.API.Controllers;

using Extensions;
using AutoMapper;
using Services;
using Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/images")]
[ApiController]
[Authorize]
public class ImagesController : ControllerBase
{
    private readonly IGalleryRepository _galleryRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMapper _mapper;

    public ImagesController(IGalleryRepository galleryRepository, IWebHostEnvironment hostingEnvironment, IMapper mapper)
    {
        this._galleryRepository = galleryRepository ?? throw new ArgumentNullException(nameof(galleryRepository));
        this._hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Image>>> GetImages()
    {
        // get from repo
        var ownerId = this.HttpContext.User.GetUserId();
        var imagesFromRepo = await this._galleryRepository.GetImagesAsync(ownerId);

        // map to model
        var imagesToReturn = this._mapper.Map<IEnumerable<Image>>(imagesFromRepo);

        // return
        return this.Ok(imagesToReturn);
    }

    [HttpGet("{id}", Name = "GetImage")]
    [Authorize(Policy = "IsImageOwner")]
    public async Task<ActionResult<Image>> GetImage(Guid id)
    {          
        var ownerId = this.HttpContext.User.GetUserId();
        var imageFromRepo = await this._galleryRepository.GetImageAsync(id, ownerId);

        if (imageFromRepo == null)
        {
            return this.NotFound();
        }

        var imageToReturn = this._mapper.Map<Image>(imageFromRepo);

        return this.Ok(imageToReturn);
    }

    [HttpPost()]
    [Authorize(Policy = "ImageCreatorPolicy")]
    [Authorize(Policy = "ClientApplicationCanWrite")]
    public async Task<ActionResult<Image>> CreateImage([FromBody] ImageForCreation imageForCreation)
    {
        // Automapper maps only the Title in our configuration
        var imageEntity = this._mapper.Map<Entities.Image>(imageForCreation);

        // Create an image from the passed-in bytes (Base64), and 
        // set the filename on the image

        // get this environment's web root path (the path
        // from which static content, like an image, is served)
        var webRootPath = this._hostingEnvironment.WebRootPath;

        // create the filename
        string fileName = Guid.NewGuid().ToString() + ".jpg";
            
        // the full file path
        var filePath = Path.Combine($"{webRootPath}/images/{fileName}");

        // write bytes and auto-close stream
        await System.IO.File.WriteAllBytesAsync(filePath, imageForCreation.Bytes);

        // fill out the filename
        imageEntity.FileName = fileName;

        // ownerId should be set - can't save image in starter solution, will
        // be fixed during the course
        //imageEntity.OwnerId = ...;

        // add and save.  
        this._galleryRepository.AddImage(imageEntity);

        await this._galleryRepository.SaveChangesAsync();

        var imageToReturn = this._mapper.Map<Image>(imageEntity);

        return this.CreatedAtRoute("GetImage",
            new { id = imageToReturn.Id },
            imageToReturn);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "IsImageOwner")]
    public async Task<IActionResult> DeleteImage(Guid id)
    {            
        var ownerId = this.HttpContext.User.GetUserId();
        var imageFromRepo = await this._galleryRepository.GetImageAsync(id, ownerId);

        if (imageFromRepo == null)
        {
            return this.NotFound();
        }

        this._galleryRepository.DeleteImage(imageFromRepo);

        await this._galleryRepository.SaveChangesAsync();

        return this.NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "IsImageOwner")]
    public async Task<IActionResult> UpdateImage(Guid id, [FromBody] ImageForUpdate imageForUpdate)
    {
        var ownerId = this.HttpContext.User.GetUserId();
        var imageFromRepo = await this._galleryRepository.GetImageAsync(id, ownerId);
        if (imageFromRepo == null)
        {
            return this.NotFound();
        }

        this._mapper.Map(imageForUpdate, imageFromRepo);

        this._galleryRepository.UpdateImage(imageFromRepo);

        await this._galleryRepository.SaveChangesAsync();

        return this.NoContent();
    }
}