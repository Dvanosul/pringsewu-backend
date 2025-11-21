
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.API.Models.UserUserType;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("employeerole", "Employee Role data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/employee/role")]
    public class EmployeeRoleController : BaseUserTypeRoleController<UserUserTypeRequest, RolePaginationDTO>
    {
        public EmployeeRoleController(IConfiguration configuration, IEmployeeRoleService baseUserTypeRoleService) : base(configuration, baseUserTypeRoleService)
        {
        }
    }
}
