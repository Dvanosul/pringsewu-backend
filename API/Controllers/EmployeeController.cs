
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.app015.Application.DTOs.Employee;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Mapster;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.app015.API.Models.Employee;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("employee", "Employee data...")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IConfiguration configuration, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BaseRequest<CreateEmployeeRequest> request)
        {
            var param = request.Data.Adapt<CreateEmployeeParam>();
            await _employeeService.CreateAsync(param);
            return Ok(ResponseHelper.Success(null, "Create employee successfully", _employeeService.GetInfo(), request));
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] BaseRequest<UpdateEmployeeRequest> request, [FromRoute] Guid id)
        {
            var param = request.Data.Adapt<UpdateEmployeeParam>();
            await _employeeService.UpdateAsync(param, id);
            return Ok(ResponseHelper.Success(null, "Update employee successfully", _employeeService.GetInfo(), request));
        }

        [Event("view")]
        [HttpPost]
        [Route("pagination")]
        public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
        {
            var response = await _employeeService.GetPaginationAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _employeeService.GetInfo()));
        }

        [Event("view")]
        [HttpPost]
        [Route("available/pagination")]
        public async Task<IActionResult> GetAvailableListPagination([FromBody] BaseRequest<PaginationQuery> request, [FromQuery] Guid? userId = null)
        {
            var response = await _employeeService.GetAvailablePaginationAsync(request.Data, userId);
            return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _employeeService.GetInfo()));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _employeeService.GetAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Get employee successfully", _employeeService.GetInfo()));
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var customer = await _employeeService.DeleteAsync(id);
            return Ok(ResponseHelper.Success<object>(customer, "Delete employee successfully", _employeeService.GetInfo()));
        }
    }
}
