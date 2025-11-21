
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("language", "Language data...")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/language")]
    public class LanguageController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILanguageService _languageService;
        public LanguageController(IConfiguration configuration, ILanguageService languageService)
        {
            _configuration = configuration;
            _languageService = languageService;
        }


        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _languageService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _languageService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _languageService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get language successfully", _languageService.GetInfo()));
        }
    }
}
