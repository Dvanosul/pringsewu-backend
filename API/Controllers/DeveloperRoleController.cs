
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.UserUserType;
using Sindika.AspNet.app015.Application.DTOs.Role;
namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("developerrole", "Developer Role data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/developer/role")]
    public class DeveloperRoleController : BaseUserTypeRoleController<UserUserTypeRequest, RolePaginationDTO>
    {
        public DeveloperRoleController(IConfiguration configuration, IDeveloperRoleService baseUserTypeRoleService) : base(configuration, baseUserTypeRoleService)
        {
        }
    }
}
