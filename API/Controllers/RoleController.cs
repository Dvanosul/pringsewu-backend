
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Mapster;
using Sindika.AspNet.app015.API.Models.Role;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("role", "Role data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/role")]
    public class RoleController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;
        public RoleController(IConfiguration configuration, IRoleService roleService)
        {
            _configuration = configuration;
            _roleService = roleService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateRoleRequest> request)
        {
            var param = request.Data.Adapt<RoleParam>();
            await _roleService.CreateAsync(param);
            return Ok(ResponseHelper.Success(null, "Create role successfully", _roleService.GetInfo(), request));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<CreateRoleRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<RoleParam>();
            await _roleService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success(null, "Update role successfully", _roleService.GetInfo(), request));
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _roleService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _roleService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _roleService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get role successfully", _roleService.GetInfo()));
        }


        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _roleService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete profile successfully", _roleService.GetInfo()));
        }
    }
}
