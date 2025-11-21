using Sindika.AspNet.app015.Application.DTOs.Event;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Sindika.AspNet.Common.Utilities;
using Mapster;

namespace Sindika.AspNet.app015.Application.Services
{
    public class EventService : BaseCrudService<
        EventService,
        Context,
        EventDTO,
        EventPaginationDTO,
        EventParam,
        Event,
        IEventRepository>, IEventService
    {
        public EventService(
            IConfiguration configuration,
            ILogger<EventService> logger,
            IUnitOfWork<Context>
            unitOfWork, IEventRepository repository
            ) : base(configuration, logger, unitOfWork, repository)
        {
        }

        public new async Task<PaginationResponse<EventPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            var orderMap = new Dictionary<string, int>
            {
                { "view", 1 },
                { "insert", 2 },
                { "update", 3 },
                { "upsert", 4 },
                { "delete", 5 },
                { "history", 6 }
            };

            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery);
                var items = itemCountResponse.Items.OrderBy(e => orderMap.GetValueOrDefault(e.Code, int.MaxValue))
                                                   .Adapt<List<EventPaginationDTO>>();
                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select(c => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }
    }
}
