
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.UserUserType;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Controller]
    public abstract class BaseUserTypeRoleController<TRequest, TPaginationDTO> : ControllerBase where TRequest : UserUserTypeRequest where TPaginationDTO : PaginationBaseItem
    {
        protected readonly IConfiguration _configuration;
        protected readonly IBaseUserUserTypeService<TPaginationDTO> _baseUserTypeRoleService;
        public BaseUserTypeRoleController(IConfiguration configuration, IBaseUserUserTypeService<TPaginationDTO> baseUserTypeRoleService)
        {
            _configuration = configuration;
            _baseUserTypeRoleService = baseUserTypeRoleService;
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetRolesPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _baseUserTypeRoleService.GetRolePaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _baseUserTypeRoleService.GetInfo()));
        }

        [Event("view")]
        [HttpPost]
        [Route("{id}/pagination")]
        public async Task<IActionResult> GetListPagination([FromRoute] Guid id, [FromBody] BaseRequest<PaginationQuery> request, [FromQuery] bool showAll = false)
        {
            var response = showAll ? _baseUserTypeRoleService.GetRolePaginationAsync(request.Data, id)
                                    : _baseUserTypeRoleService.GetEnabledRolePaginationAsync(request.Data, id);
            return Ok(ResponseHelper.Success<object>(await response, "Get paginated successfully", _baseUserTypeRoleService.GetInfo()));
        }
    }
}
