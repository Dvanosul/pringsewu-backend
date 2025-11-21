
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("zone", "Zone data...")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/zone")]
    public class ZoneController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IZoneService _zoneService;
        public ZoneController(IConfiguration configuration, IZoneService zoneService)
        {
            _configuration = configuration;
            _zoneService = zoneService;
        }


        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _zoneService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _zoneService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _zoneService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get zone successfully", _zoneService.GetInfo()));
        }
    }
}
