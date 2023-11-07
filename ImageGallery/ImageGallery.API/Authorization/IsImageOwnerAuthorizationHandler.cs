namespace ImageGallery.API.Authorization;

using Microsoft.AspNetCore.Authorization;
using Extensions;
using Services;

public class IsImageOwnerAuthorizationHandler : AuthorizationHandler<IsImageOwnerAuthorizationRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IGalleryRepository _galleryRepository;

    public IsImageOwnerAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IGalleryRepository galleryRepository)
    {
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this._galleryRepository = galleryRepository ?? throw new ArgumentNullException(nameof(galleryRepository));
    }

    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsImageOwnerAuthorizationRequirement requirement)
    {
        var imageIdAsString = this._httpContextAccessor.HttpContext.GetRouteValue("id")?.ToString();
        if (!Guid.TryParse(imageIdAsString, out var imageId))
        {
            context.Fail();
            return;
        }

        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            context.Fail();
            return;
        }

        var isOwner = await this._galleryRepository.IsImageOwnerAsync(imageId, userId).ConfigureAwait(false);
        if (!isOwner)
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }
    }
}