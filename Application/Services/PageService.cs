using Mapster;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Authentication.Interfaces;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.Event;

namespace Sindika.AspNet.app015.Application.Services
{
    public class PageService : BaseCrudService<
        PageService,
        Context,
        PageDTO,
        PagePaginationDTO,
        PageParam,
        Page,
        IPageRepository>, IPageService
    {
        private readonly IPageEventRepository _pageEventRepository;
        private readonly IProfilePageEventRepository _profilePageEventRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IRoleProfileRepository _roleProfileRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ISynchronizeCache _synchronizeCache;

        public PageService(
            IConfiguration configuration,
            ILogger<PageService> logger,
            IUnitOfWork<Context>
            unitOfWork, IPageRepository repository,
            IPageEventRepository pageEventRepository,
            IProfilePageEventRepository profilePageEventRepository,
            IProfileRepository profileRepository,
            IRoleProfileRepository roleProfileRepository,
            IEventRepository eventRepository,
            ISynchronizeCache synchronizeCache
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _pageEventRepository = pageEventRepository;
            _profilePageEventRepository = profilePageEventRepository;
            _profileRepository = profileRepository;
            _roleProfileRepository = roleProfileRepository;
            _eventRepository = eventRepository;
            _synchronizeCache = synchronizeCache;
        }

        public async Task<PaginationResponse<PageWithEventPaginationDTO>> GetCustomPaginationAsync(Guid profileId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                // Pagination page
                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery);
                var items = itemCountResponse.Items.Adapt<List<PageWithEventPaginationDTO>>();
                var ids = itemCountResponse.Items.Select((p) => p.Id).ToList();

                // Get page events for all page in the current pagination
                var pageEvents = await _pageEventRepository.GetsAsync(query => query.Where(pe => ids.Contains(pe.PageId) && pe.Event!.IsDefault)
                                                                                  .Include(pe => pe.Event));
                // Get active profile for all page events in the current pagination
                var profiles = await _profilePageEventRepository.GetsAsync(query => query.Where(ppe => ppe.ProfileId == profileId && ids.Contains(ppe.PageId)));


                var enabledCustomEventsDict = await _profilePageEventRepository.GetPagesEnabledCustomEventCountAsync(profileId, ids);
                var customEventsDict = await _pageEventRepository.GetPagesCustomEventCount(ids);

                var pageEventsDict = pageEvents.GroupBy(p => p.PageId)
                                               .ToDictionary(c => c.Key, c => c.ToList());
                var profilesDict = profiles.GroupBy(p => new { p.PageId, p.EventId })
                                           .ToDictionary(c => c.Key, c => c.FirstOrDefault());


                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items.Select(p =>
                {
                    p.EnabledCustomEventsCount = enabledCustomEventsDict.GetValueOrDefault(p.Id, 0);
                    p.CustomEventsCount = customEventsDict.GetValueOrDefault(p.Id, 0);

                    if (pageEventsDict.TryGetValue(p.Id, out var pageEvents))
                    {
                        p.Events = pageEvents.Select(pe =>
                        {
                            var pageEvent = pe.Adapt<PageEventDTO>();
                            pageEvent.IsEnabled = profilesDict.ContainsKey(new { PageId = p.Id, pe.EventId });

                            return new { Id = pe.EventId, Data = pageEvent };
                        }).ToDictionary(c => c.Id, c => c.Data);
                    }

                    return p;
                }).ToList(), itemCountResponse.Count);
                AppendRecords(null, ids.Adapt<List<string>>());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<PaginationResponse<EventPaginationDTO>> GetPageEventsPaginationAsync(Guid pageId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                paginationQuery.AddDefaultSort("name", "asc");
                var itemCountResponse = await _pageEventRepository.GetPaginationAsync(paginationQuery, q =>
                {
                    return q.Where(pe => pe.PageId == pageId && pe.HasEvent)
                            .Select(pe => pe.Event)
                            .ProjectToType<EventPaginationDTO>();
                });
                var ids = itemCountResponse.Items.Select((p) => p.Id).ToList();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, itemCountResponse.Items, itemCountResponse.Count);
                AppendRecords(null, ids.Adapt<List<string>>());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<PaginationResponse<PageCustomEventDTO>> GetCustomEventsPaginationAsync(Guid profileId, Guid pageId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                // Pagination page
                var itemCountResponse = await _pageEventRepository.GetPaginationAsync(paginationQuery, q =>
                {
                    return q.Where(pe => pe.PageId == pageId
                                         && !pe.Event!.IsDefault
                                         && pe.HasEvent).Include(pe => pe.Event);
                });
                var eventIds = itemCountResponse.Items.Select(p => p.EventId);

                var enabledEvents = await _profilePageEventRepository.GetPageEnabledCustomEventIdsAsync([profileId], pageId, eventIds);

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, itemCountResponse.Items.Select(p =>
                {
                    var item = p.Event.Adapt<PageCustomEventDTO>();
                    item.IsEnabled = enabledEvents.Contains(p.EventId);

                    return item;
                }).ToList(), itemCountResponse.Count);
                AppendRecords(null, eventIds.Adapt<List<string>>());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task UpsertAsync(PageParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPSERT");
                var profile = await _profileRepository.GetAsync(param.ProfileId) ?? throw new NotFoundException("Profile not found.");

                var pageIds = param.ChangedPageEvents.Select(ppe => ppe.PageId).ToHashSet();
                var eventIds = param.ChangedPageEvents.Select(ppe => ppe.EventId).ToHashSet();

                if (!await _repository.IsAllExistAsync(pageIds.ToList()))
                {
                    throw new NotFoundException("Page not found");
                }

                if (!await _eventRepository.IsAllExistAsync(eventIds.ToList()))
                {
                    throw new NotFoundException("Event not found");
                }

                var records = await _profilePageEventRepository.UpsertAsync(param.ProfileId, [.. param.ChangedPageEvents.Select(ppe => (ppe.PageId, ppe.EventId))]);
                AppendRecords(null, records.Adapt<List<string>>());
                await _unitOfWork.CommitAsync();

                var roles = await _roleProfileRepository.GetsAsync(q => q.Where(rp => rp.ProfileId == param.ProfileId).Include(rp => rp.Role));
                var roleCodes = roles.Select(c => c.Role!.Code).ToHashSet();

                await _synchronizeCache.ClearKeys(roleCodes);
                await _synchronizeCache.GenerateRoleKeys(roleCodes);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                EndOperation();
            }

        }
    }
}
