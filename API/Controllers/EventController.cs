
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("event", "Event data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/event")]
    public class EventController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IEventService _eventService;
        public EventController(IConfiguration configuration, IEventService eventService)
        {
            _configuration = configuration;
            _eventService = eventService;
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _eventService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _eventService.GetInfo()));
        }
    }
}
