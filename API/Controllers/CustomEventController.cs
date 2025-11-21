
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.Application.DTOs.CustomEvent;
using Mapster;
using Sindika.AspNet.app015.API.Models.CustomEvent;
namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("customevent", "Custom Event data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/custom-event")]
    public class CustomEventController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly ICustomEventService _customEventService;
        public CustomEventController(IConfiguration configuration, ICustomEventService customEventService)
        {
            _configuration = configuration;
            _customEventService = customEventService;
        }


        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateCustomEventRequest> request)
        {
            var param = request.Data.Adapt<CustomEventParam>();
            await _customEventService.CreateAsync(param, request.Data.AppliedPages);
            return Ok(ResponseHelper.Success(null, "Create custom event successfully", _customEventService.GetInfo(), request));
        }


        [Event("view")]
        [HttpPost]
        [Route("applied-pages/{id}")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request, [FromRoute] Guid id)
        {
            var response = await _customEventService.GetAppliedPages(id, request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _customEventService.GetInfo()));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<UpdateCustomEventRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<CustomEventParam>();
            await _customEventService.UpdateAsync(param, id, request.Data.ChangedAppliedPages);
            return Ok(ResponseHelper.Success(null, "Update custom event successfully", _customEventService.GetInfo(), request));
        }


        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _customEventService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _customEventService.GetInfo()));
        }


        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _customEventService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete custom event successfully", _customEventService.GetInfo()));
        }
    }
}
