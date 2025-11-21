
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("usertype", "UserType data...")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/usertype")]
    public class UserTypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserTypeService _usertypeService;
        public UserTypeController(IConfiguration configuration, IUserTypeService usertypeService)
        {
            _configuration = configuration;
            _usertypeService = usertypeService;
        }


        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _usertypeService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _usertypeService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _usertypeService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get usertype successfully", _usertypeService.GetInfo()));
        }
    }
}
