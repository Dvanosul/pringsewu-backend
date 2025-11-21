
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.Application.DTOs.RoleProfile;
using Mapster;
using Sindika.AspNet.app015.API.Models.RoleProfile;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("roleprofile", "Role profile data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/role-profile")]
    public class RoleProfileController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IRoleProfileService _roleProfileService;
        public RoleProfileController(IConfiguration configuration, IRoleProfileService roleProfileService)
        {
            _configuration = configuration;
            _roleProfileService = roleProfileService;
        }

        [Event("view")]
        [HttpPost]
        [Route("role/{roleId}/pagination")]
        public async Task<IActionResult> GetListPagination([FromRoute] Guid roleId, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _roleProfileService.GetCustomPaginationAsync(roleId, request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _roleProfileService.GetInfo()));
        }

        [Event("upsert")]
        [HttpPut]
        [Route("upsert")]
        public async Task<IActionResult> Upsert([FromBody] BaseRequest<UpsertRoleProfileRequest> request)
        {
            var param = request.Data.Adapt<RoleProfileParam>();
            await _roleProfileService.UpsertAsync(param);
            return Ok(ResponseHelper.Success<object>(null, "Upsert role profile successfully", _roleProfileService.GetInfo()));
        }
    }
}
