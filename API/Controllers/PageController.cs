
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Mapster;
using Sindika.AspNet.app015.API.Models.Page;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("page", "Page data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/page")]
    public class PageController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IPageService _pageService;
        public PageController(IConfiguration configuration, IPageService pageService)
        {
            _configuration = configuration;
            _pageService = pageService;
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _pageService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _pageService.GetInfo()));
        }

        [Event("view")]
        [HttpPost]
        [Route("profile/{profileId}/pagination")]
        public async Task<IActionResult> GetListPagination([FromRoute] Guid profileId, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _pageService.GetCustomPaginationAsync(profileId, request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _pageService.GetInfo()));
        }

        [Event("view")]
        [HttpPost]
        [Route("custom-event/{pageId}/profile/{profileId}/pagination")]
        public async Task<IActionResult> GetCustomEventListPagination([FromRoute] Guid pageId, [FromRoute] Guid profileId, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _pageService.GetCustomEventsPaginationAsync(profileId, pageId, request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _pageService.GetInfo()));
        }

        [Event("view")]
        [HttpPost]
        [Route("event/{pageId}/pagination")]
        public async Task<IActionResult> GetPageEventListPagination([FromRoute] Guid pageId, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _pageService.GetPageEventsPaginationAsync(pageId, request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _pageService.GetInfo()));
        }

        [Event("upsert")]
        [HttpPut]
        [Route("upsert")]
        public async Task<IActionResult> Upsert([FromBody] BaseRequest<UpsertProfilePageEventRequest> request)
        {
            var param = request.Data.Adapt<PageParam>();
            await _pageService.UpsertAsync(param);
            return Ok(ResponseHelper.Success<object>(null, "Upsert role profile successfully", _pageService.GetInfo()));
        }
    }
}
