using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Menu;
using Sindika.AspNet.app015.Application.DTOs.Menu;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.Exceptions.Unauthorized;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("menu", "Menu data ..")]
    [ApiController]
    [Route("api/v1/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMenuService _menuService;

        public MenuController(IConfiguration configuration, IMenuService menuService)
        {
            _configuration = configuration;
            _menuService = menuService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var userId = User.FindFirst("user_id")?.Value.Adapt<Guid>()
                         ?? throw new UnauthorizedAccessAttemptException();
            if (!Request.Headers.TryGetValue("X-Role-Active", out var headerValue) || string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException("X-Role-Active is missing.");
            }
            var data = await _menuService.GetUserMenuListAsync(userId, headerValue.ToString());

            return Ok(ResponseHelper.Success<object>(data, "Get menu list successfully", _menuService.GetInfo()));
        }

        [Event("insert")]
        [HttpPost("create")]
        [PrivateScope]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateMenuRequest> request)
        {
            var param = request.Data.Adapt<MenuParam>();
            await _menuService.CreateAsync(param);
            return Ok(ResponseHelper.Success(null, "Create menu successfully", _menuService.GetInfo(), request));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        [PrivateScope]
        public async Task<IActionResult> Update([FromBody] BaseRequest<CreateMenuRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<MenuParam>();
            await _menuService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success(null, "Update menu successfully", _menuService.GetInfo(), request));
        }

        [Event("view")]
        [HttpPost("all")]
        [PrivateScope]
        public async Task<IActionResult> GetListPagination()
        {
            var response = await _menuService.GetAllMenuListAsync();
            return Ok(ResponseHelper.Success<object>(response, "Get all menu successfully", _menuService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        [PrivateScope]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _menuService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get menu successfully", _menuService.GetInfo()));
        }

        [Event("update")]
        [HttpPost("move/{id}")]
        [PrivateScope]
        public async Task<IActionResult> Move([FromBody] MoveMenuRequest request, [FromRoute] Guid id)
        {
            var param = request.Adapt<MoveMenuParam>();
            await _menuService.MoveAsync(param, id);
            return Ok(ResponseHelper.Success<object>(null, "Moved menu successfully", _menuService.GetInfo()));
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        [PrivateScope]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _menuService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete menu successfully", _menuService.GetInfo()));
        }
    }
}
