using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Auth;
using Sindika.AspNet.app015.Application.DTOs.Auth;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.Authentication.Interfaces;
using Sindika.AspNet.Exceptions.BadRequest;
using Sindika.AspNet.Exceptions.Unauthorized;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISSOService _sSOService;
        private readonly IAuthService _authService;
        private readonly IUserInfoService _userInfoService;

        public AuthController(
            IConfiguration configuration,
            IAuthService authService,
            ISSOService sSOService,
            IUserInfoService userInfoService)
        {
            _configuration = configuration;
            _sSOService = sSOService;
            _authService = authService;
            _userInfoService = userInfoService;
        }

        [HttpGet("generate")]
        [PublicScope]
        public IActionResult Generate([FromQuery] string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                return BadRequest(ResponseHelper.Error<object, object>(null, "ERR-005-BADREQUEST", "Platform query parameter is required."));
            }
            var data = _sSOService.GenerateAuthUrl(platform);
            return Ok(ResponseHelper.Success<object>(data, "Generate auth url successfully", null, null));
        }

        [HttpPost]
        [Route("token")]
        [PublicScope]
        public async Task<IActionResult> ExchangeCodeForAccessToken([FromBody] BaseRequest<CreateTokenRequest> request)
        {
            var param = request.Data.Adapt<TokenParam>();
            var tokenDTO = await _authService.GetToken(param);

            return Ok(ResponseHelper.Success<object>(tokenDTO, "Get access token successfully", _authService.GetInfo()));
        }

        [HttpGet("logout")]
        [PublicScope]
        public IActionResult logout([FromQuery] string platform, [FromQuery] string? idTokenHint)
        {
            if (string.IsNullOrEmpty(platform))
            {
                return BadRequest(ResponseHelper.Error<object, object>(null, "ERR-005-BADREQUEST", "Platform query parameter is required."));
            }
            var data = _sSOService.GenerateLogoutUrl(platform);
            if (idTokenHint != null)
            {
                data += "&id_token_hint=" + idTokenHint;
            }
            return Ok(ResponseHelper.Success<object>(data, "Generate logout url successfully", null, null));
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirst("user_id")?.Value.Adapt<Guid>()
                         ?? throw new UnauthorizedAccessAttemptException();
            var data = await _userInfoService.GetUserInfo(userId);

            return Ok(ResponseHelper.Success<object>(data, "Get user info successfully", null, null));
        }

        [HttpPost("roles")]
        public async Task<IActionResult> GetUserRoles([FromBody] BaseRequest<PaginationQuery> request)
        {
            var userId = User.FindFirst("user_id")?.Value.Adapt<Guid>()
                         ?? throw new UnauthorizedAccessAttemptException();
            var data = await _userInfoService.GetUserRolesPaginationAsync(userId, request.Data);

            return Ok(ResponseHelper.Success<object>(data, "Get user roles successfully", null, null));
        }

        [HttpPost("roles/{roleId}/page/pagination")]
        public async Task<IActionResult> GetRolePageAccess([FromRoute] Guid roleId, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var data = await _userInfoService.GetRolePageAccessPaginationAsync(roleId, request.Data);

            return Ok(ResponseHelper.Success<object>(data, "Get role page access successfully", null, null));
        }

        [HttpPost("roles/{roleId}/page/{pageId}/event/pagination")]
        public async Task<IActionResult> GetRolePageEventAccess([FromRoute] Guid roleId, [FromRoute] Guid pageId, [FromBody] BaseRequest<PaginationQuery> request)
        {
            var data = await _userInfoService.GetRolePageEventAccessPaginationAsync(roleId, pageId, request.Data);

            return Ok(ResponseHelper.Success<object>(data, "Get role page event access successfully", null, null));
        }

        [HttpPost]
        [Route("login")]
        [PublicScope]
        public async Task<IActionResult> Login([FromBody] BaseRequest<LoginRequest> request)
        {
            var param = request.Data.Adapt<LoginParam>();
            var tokenDTO = await _authService.LoginAsync(param);

            return Ok(
                ResponseHelper.Success<object>(
                    tokenDTO,
                    "Login successfully",
                    _authService.GetInfo()
                )
            );
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] BaseRequest<ChangePasswordRequest> request
        )
        {
            var userEmail =
                User.FindFirst(ClaimTypes.Email)?.Value
                ?? throw new UnauthorizedAccessAttemptException();

            if (request.Data.Password != request.Data.ConfirmationPassword)
            {
                throw new BadRequestException(
                    "VAL-GEN-002",
                    "Password and confirmation password do not match."
                );
            }

            await _authService.ChangePasswordAsync(userEmail, request.Data.Password);
            return Ok(
                ResponseHelper.Success<object>(null, "Password changed successfully", null, null)
            );
        }
    }
}
