using Sindika.AspNet.app015.Application.DTOs.Developer;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IDeveloperService : IBaseCrudService<DeveloperDTO, DeveloperPaginationDTO, CreateDeveloperParam>
    {
        Task<Guid> UpdateAsync(UpdateDeveloperParam param, Guid id);
        Task<PaginationResponse<DeveloperPaginationDTO>> GetAvailablePaginationAsync(PaginationQuery paginationQuery, Guid? userId = null);
    }
}
