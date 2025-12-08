using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.DonationEvent;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.DonationEvent;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("donation-event", "Donation event management")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/donation-event")]
    public class DonationEventController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDonationEventService _donationEventService;

        public DonationEventController(
            IConfiguration configuration,
            IDonationEventService donationEventService)
        {
            _configuration = configuration;
            _donationEventService = donationEventService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateDonationEventRequest request)
        {
            var param = request.Adapt<CreateDonationEventParam>();
            var id = await _donationEventService.CreateWithImageAsync(param, request.Image);
            return Ok(ResponseHelper.Success<object>(new { Id = id }, "Create donation event successfully", _donationEventService.GetInfo()));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateDonationEventRequest request, [FromRoute] Guid id)
        {
            var param = request.Adapt<UpdateDonationEventParam>();
            await _donationEventService.UpdateWithImageAsync(param, id, request.Image);
            return Ok(ResponseHelper.Success<object>(null, "Update donation event successfully", _donationEventService.GetInfo()));
        }

        [Event("update")]
        [HttpPut("update/{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromBody] BaseRequest<UpdateDonationEventStatusRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<UpdateDonationEventStatusParam>();
            await _donationEventService.UpdateStatusAsync(param, id);
            return Ok(ResponseHelper.Success<object>(null, "Update donation event status successfully", _donationEventService.GetInfo()));
        }

        [Event("view")]
        [HttpPost("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _donationEventService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _donationEventService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var donationEvent = await _donationEventService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(donationEvent, "Get donation event successfully", _donationEventService.GetInfo()));
        }

        [Event("view")]
        [PublicScope]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEvents()
        {
            var events = await _donationEventService.GetActiveEventsAsync();
            return Ok(ResponseHelper.Success<object>(events, "Get active donation events successfully", _donationEventService.GetInfo()));
        }

        [Event("view")]
        [PublicScope]
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id)
        {
            var result = await _donationEventService.GetImageStreamAsync(id);
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
            var deletedId = await _donationEventService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(new { Id = deletedId }, "Delete donation event successfully", _donationEventService.GetInfo()));
        }
    }
}
