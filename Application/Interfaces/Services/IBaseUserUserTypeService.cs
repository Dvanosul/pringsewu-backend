using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IBaseUserUserTypeService<TPaginationDTO> : IBaseService where TPaginationDTO : PaginationBaseItem
    {
        Task<PaginationResponse<TPaginationDTO>> GetRolePaginationAsync(PaginationQuery paginationQuery, Guid? identifier = null);
        Task<PaginationResponse<TPaginationDTO>> GetEnabledRolePaginationAsync(PaginationQuery paginationQuery, Guid identifier);
    }
}
