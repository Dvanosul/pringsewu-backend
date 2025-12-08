using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.Category;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.API.Models.Category;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("category", "Category management")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryService _categoryService;

        public CategoryController(IConfiguration configuration, ICategoryService categoryService)
        {
            _configuration = configuration;
            _categoryService = categoryService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateCategoryRequest> request)
        {
            var param = request.Data.Adapt<CategoryParam>();
            var id = await _categoryService.CreateAsync(param);
            return Ok(ResponseHelper.Success<object>(new { Id = id }, "Create category successfully", _categoryService.GetInfo()));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<UpdateCategoryRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<CategoryParam>();
            await _categoryService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success<object>(null, "Update category successfully", _categoryService.GetInfo()));
        }

        [Event("view")]
        [HttpPost("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _categoryService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _categoryService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var category = await _categoryService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(category, "Get category successfully", _categoryService.GetInfo()));
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedId = await _categoryService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(new { Id = deletedId }, "Delete category successfully", _categoryService.GetInfo()));
        }
    }
}
