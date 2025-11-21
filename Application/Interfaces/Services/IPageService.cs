using Sindika.AspNet.app015.Application.DTOs.Event;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IPageService : IBaseCrudService<PageDTO, PagePaginationDTO, PageParam>
    {
        Task<PaginationResponse<PageWithEventPaginationDTO>> GetCustomPaginationAsync(Guid profileId, PaginationQuery paginationQuery);
        Task<PaginationResponse<EventPaginationDTO>> GetPageEventsPaginationAsync(Guid pageId, PaginationQuery paginationQuery);
        Task<PaginationResponse<PageCustomEventDTO>> GetCustomEventsPaginationAsync(Guid profileId, Guid pageId, PaginationQuery paginationQuery);
        Task UpsertAsync(PageParam param);
    }
}
