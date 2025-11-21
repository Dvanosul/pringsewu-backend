using Sindika.AspNet.app015.Application.DTOs.CustomEvent;
using Sindika.AspNet.app015.Application.DTOs.PageEvent;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface ICustomEventService : IBaseCrudService<CustomEventDTO, CustomEventPaginationDTO, CustomEventParam>
    {
        public Task<PaginationResponse<PageEventPaginationDTO>> GetAppliedPages(Guid eventId, PaginationQuery paginationQuery);
        public Task<Guid> CreateAsync(CustomEventParam param, IEnumerable<Guid> appliedPages);
        public Task<Guid> UpdateAsync(CustomEventParam param, Guid id, IEnumerable<Guid> changedAppliedPages);
    }
}
