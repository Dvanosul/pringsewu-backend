
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.User;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.User;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("user", "User data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateUserRequest> request)
        {
            var param = request.Data.Adapt<CreateUserParam>();
            await _userService.CreateAsync(param);
            return Ok(ResponseHelper.Success(null, "Create user successfully", _userService.GetInfo(), request));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<CreateUserRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<CreateUserParam>();
            await _userService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success(null, "Update user successfully", _userService.GetInfo(), request));
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _userService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _userService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _userService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get user successfully", _userService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}/role-profile")]
        public async Task<IActionResult> GetRoleProfile([FromRoute] Guid id)
        {
            var customer = await _userService.GetUserTypesRolesAndProfilesAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get user roles and profiles successfully", _userService.GetInfo()));
        }

        [Event("view")]
        [HttpPost("page-access/{id}/pagination")]
        public async Task<IActionResult> PageAccessPagination([FromRoute] Guid id, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _userService.GetUserPageAccessPaginationAsync(id, request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _userService.GetInfo()));
        }

        [Event("view")]
        [HttpPost("page-access/{id}/custom-event/{pageId}/pagination")]
        public async Task<IActionResult> PageCustomEventAccessPagination([FromRoute] Guid id, [FromRoute] Guid pageId, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _userService.GetUserPageCustomEventAccessPaginationAsync(id, pageId, request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _userService.GetInfo()));
        }


        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _userService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete user successfully", _userService.GetInfo()));
        }
    }
}
