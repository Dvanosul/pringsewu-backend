using Sindika.AspNet.app015.Application.DTOs.Employee;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IEmployeeService : IBaseCrudService<EmployeeDTO, EmployeePaginationDTO, CreateEmployeeParam>
    {
        Task<PaginationResponse<EmployeePaginationDTO>> GetAvailablePaginationAsync(PaginationQuery paginationQuery, Guid? userId = null);
        Task<Guid> UpdateAsync(UpdateEmployeeParam param, Guid id);
    }
}
