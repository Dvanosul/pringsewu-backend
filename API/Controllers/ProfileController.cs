
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.Profile;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.Profile;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("profile", "Profile data ..")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/profile")]
    public class ProfileController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IProfileService _profileService;
        public ProfileController(IConfiguration configuration, IProfileService profileService)
        {
            _configuration = configuration;
            _profileService = profileService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateProfileRequest> request)
        {
            var param = request.Data.Adapt<ProfileParam>();
            await _profileService.CreateAsync(param);
            return Ok(ResponseHelper.Success(null, "Create profile successfully", _profileService.GetInfo(), request));
        }

        [Event("insert")]
        [HttpPost("duplicate/{id}")]
        public async Task<IActionResult> Duplicate([FromRoute] Guid id)
        {
            await _profileService.DuplicateAsync(id);
            return Ok(ResponseHelper.Success<object>(null, "Create profile successfully", _profileService.GetInfo()));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<CreateProfileRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<ProfileParam>();
            await _profileService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success(null, "Update profile successfully", _profileService.GetInfo(), request));
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _profileService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _profileService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _profileService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get profile successfully", _profileService.GetInfo()));
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _profileService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete profile successfully", _profileService.GetInfo()));
        }
    }
}
