
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.Developer;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.Developer;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("developer", "Developer data...")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/developer")]
    public class DeveloperController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDeveloperService _developerService;
        public DeveloperController(IConfiguration configuration, IDeveloperService developerService)
        {
            _configuration = configuration;
            _developerService = developerService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateDeveloperRequest> request)
        {
            var param = request.Data.Adapt<CreateDeveloperParam>();
            await _developerService.CreateAsync(param);
            return Ok(ResponseHelper.Success(null, "Create developer successfully", _developerService.GetInfo(), request));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<UpdateDeveloperRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<UpdateDeveloperParam>();
            await _developerService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success(null, "Update developer successfully", _developerService.GetInfo(), request));
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _developerService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _developerService.GetInfo()));
        }


        [Event("view")]
        [HttpPost]
        [Route("available/pagination")]
        public async Task<IActionResult> GetAvailableListPagination([FromBody] BaseRequest<PaginationQuery> request, [FromQuery] Guid? userId = null)
        {
            var response = await _developerService.GetAvailablePaginationAsync(request.Data, userId);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _developerService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _developerService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get developer successfully", _developerService.GetInfo()));
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _developerService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete developer successfully", _developerService.GetInfo()));
        }
    }
}
