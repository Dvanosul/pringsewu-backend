using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("gender", "Gender data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/gender")]
    public class GenderController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IGenderService _genderService;
        public GenderController(IConfiguration configuration, IGenderService genderService)
        {
            _configuration = configuration;
            _genderService = genderService;
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _genderService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _genderService.GetInfo()));
        }
    }
}
