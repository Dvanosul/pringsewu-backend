using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.DonationGallery;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.DonationGallery;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("donation-gallery", "Donation gallery management")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/donation-gallery")]
    public class DonationGalleryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDonationGalleryService _donationGalleryService;

        public DonationGalleryController(
            IConfiguration configuration,
            IDonationGalleryService donationGalleryService)
        {
            _configuration = configuration;
            _donationGalleryService = donationGalleryService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateDonationGalleryRequest request)
        {
            var param = request.Adapt<CreateDonationGalleryParam>();
            var id = await _donationGalleryService.CreateWithImageAsync(param, request.Image);
            return Ok(ResponseHelper.Success<object>(new { Id = id }, "Create donation gallery successfully", _donationGalleryService.GetInfo()));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateDonationGalleryRequest request, [FromRoute] Guid id)
        {
            var param = request.Adapt<UpdateDonationGalleryParam>();
            await _donationGalleryService.UpdateWithImageAsync(param, id, request.Image);
            return Ok(ResponseHelper.Success<object>(null, "Update donation gallery successfully", _donationGalleryService.GetInfo()));
        }

        [Event("view")]
        [HttpPost("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _donationGalleryService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _donationGalleryService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var gallery = await _donationGalleryService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(gallery, "Get donation gallery successfully", _donationGalleryService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEventId([FromRoute] Guid eventId)
        {
            var galleries = await _donationGalleryService.GetByEventIdAsync(eventId);
            return Ok(ResponseHelper.Success<object>(galleries, "Get donation galleries by event successfully", _donationGalleryService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("event/code/{eventCode}")]
        public async Task<IActionResult> GetByEventCode([FromRoute] string eventCode)
        {
            var galleries = await _donationGalleryService.GetByEventCodeAsync(eventCode);
            return Ok(ResponseHelper.Success<object>(galleries, "Get donation galleries by event code successfully", _donationGalleryService.GetInfo()));
        }

        [Event("view")]
        [PublicScope]
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id)
        {
            var result = await _donationGalleryService.GetImageStreamAsync(id);
            if (result.Stream == null)
            {
                return NotFound(ResponseHelper.Error<object, object>(null, "ERR-404", "Image not found"));
            }

            return File(result.Stream, result.ContentType);
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedId = await _donationGalleryService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(new { Id = deletedId }, "Delete donation gallery successfully", _donationGalleryService.GetInfo()));
        }
    }
}
