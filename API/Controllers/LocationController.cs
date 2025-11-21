using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.Application.DTOs.Location;
using Sindika.AspNet.app015.API.Models.Location;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("location", "Location data...")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/location")]
    public class LocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILocationService _locationService;
        public LocationController(IConfiguration configuration, ILocationService locationService)
        {
            _configuration = configuration;
            _locationService = locationService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateLocationRequest> request)
        {
            var param = request.Data.Adapt<LocationParam>();
            await _locationService.CreateAsync(param);
            return Ok(ResponseHelper.Success(null, "Create location successfully", _locationService.GetInfo(), request));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<UpdateLocationRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<LocationParam>();
            param.Id = id;
            await _locationService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success(null, "Update location successfully", _locationService.GetInfo(), request));
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _locationService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _locationService.GetInfo()));
        }


        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _locationService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get location successfully", _locationService.GetInfo()));
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _locationService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete location successfully", _locationService.GetInfo()));
        }
    }
}
