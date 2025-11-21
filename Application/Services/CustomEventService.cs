using Sindika.AspNet.app015.Application.DTOs.CustomEvent;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Mapster;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.app015.Application.DTOs.PageEvent;
using Microsoft.EntityFrameworkCore;

namespace Sindika.AspNet.app015.Application.Services
{
    public class CustomEventService : BaseCrudService<
        CustomEventService,
        Context,
        CustomEventDTO,
        CustomEventPaginationDTO,
        CustomEventParam,
        Event,
        IEventRepository>, ICustomEventService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IPageEventRepository _pageEventRepository;
        private readonly IProfilePageEventRepository _profilePageEventRepository;

        public CustomEventService(
            IConfiguration configuration,
            ILogger<CustomEventService> logger,
            IUnitOfWork<Context>
            unitOfWork, IEventRepository repository,
            IPageRepository pageRepository,
            IPageEventRepository pageEventRepository,
            IProfilePageEventRepository profilePageEventRepository
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _pageRepository = pageRepository;
            _pageEventRepository = pageEventRepository;
            _profilePageEventRepository = profilePageEventRepository;
        }

        public async Task<PaginationResponse<PageEventPaginationDTO>> GetAppliedPages(Guid eventId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                var isEventValid = await _repository.IsExistAsync(eventId);
                if (!isEventValid)
                {
                    throw new NotFoundException("Custom event not found.");
                }

                var itemCountResponse = await _pageEventRepository.GetPaginationAsync(paginationQuery, q => q.Where(pe => pe.EventId == eventId)
                                                                   .Include(pe => pe.Page));
                var items = itemCountResponse.Items.Select(pe =>
                {
                    var item = pe.Page.Adapt<PageEventPaginationDTO>();
                    item.HasEvent = pe.HasEvent;

                    return item;
                }).ToList();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, items.Select(c => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }


        public async Task<Guid> CreateAsync(CustomEventParam param, IEnumerable<Guid> appliedPages)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                // Check for invalid pages
                var pages = await _pageRepository.GetsAsync();
                var missingPageIds = appliedPages.Except(pages.Select(c => c.Id));

                if (missingPageIds.Any())
                {
                    throw new NotFoundException($"Page not found: {string.Join(", ", missingPageIds)}");
                }

                // Creating the custom event
                var entity = param.Adapt<Event>();
                entity.IsDefault = false;
                var id = await _repository.CreateAsync(entity);

                // Seeding the page event for all pages
                var pageEvents = pages.Select(c => new PageEvent { EventId = id, PageId = c.Id, HasEvent = appliedPages.Contains(c.Id) }).ToList();
                await _pageEventRepository.CreateRangeAsync(pageEvents);

                AppendRecords(id.ToString());
                await _unitOfWork.CommitAsync();
                return id;
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

        public async Task<Guid> UpdateAsync(CustomEventParam param, Guid id, IEnumerable<Guid> changedAppliedPages)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                // Getting the event data and updating it
                var entity = (await _repository.GetAsync(id, query => query.Where(c => !c.IsDefault)))
                             ?? throw new NotFoundException("CustomEvent not found.");
                param.Adapt(entity);
                entity.UpdatedDate = DateTimeOffset.UtcNow;


                if (changedAppliedPages.Any())
                {
                    if (!await _pageRepository.IsAllExistAsync(changedAppliedPages.ToList()))
                    {
                        throw new NotFoundException("Page not found.");
                    }

                    // Page event data
                    var existingPageEvents = await _pageEventRepository.GetsAsync(q => q.Where(pe => pe.EventId == id && changedAppliedPages.Contains(pe.PageId)));
                    var pageEventToAdd = changedAppliedPages.Except(existingPageEvents.Select(pe => pe.PageId))
                                                            .Select(c => new PageEvent { EventId = id, PageId = c });
                    var pageEventToDelete = existingPageEvents.Where(pe => !pe.HasEvent).Select(pe => pe.PageId);

                    existingPageEvents.ForEach(pe =>
                    {
                        pe.HasEvent = !pe.HasEvent;
                        pe.UpdatedDate = DateTimeOffset.UtcNow;
                    });

                    // Profile page event data
                    var profilePageEventToDelete = await _profilePageEventRepository.GetsAsync(q => q.Where(ppe => ppe.EventId == id && pageEventToDelete.Contains(ppe.PageId)));
                    profilePageEventToDelete.ForEach(ppe =>
                    {
                        ppe.IsActive = false;
                        ppe.DeletedDate = DateTimeOffset.UtcNow;
                    });

                    if (pageEventToAdd.Any()) await _pageEventRepository.CreateRangeAsync(pageEventToAdd.ToList());
                    if (existingPageEvents.Count != 0) await _pageEventRepository.UpdateRangeAsync(existingPageEvents);
                    if (profilePageEventToDelete.Count != 0) await _profilePageEventRepository.UpdateRangeAsync(profilePageEventToDelete);
                }

                await _repository.UpdateAsync(entity);
                AppendRecords(entity.Id.ToString());
                await _unitOfWork.CommitAsync();
                return entity.Id;
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

        public new async Task<PaginationResponse<CustomEventPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery, q => q.Where(e => !e.IsDefault));
                var ids = itemCountResponse.Items.Select(c => c.Id);
                var items = itemCountResponse.Items.Adapt<List<CustomEventPaginationDTO>>();

                var appliedPageCountDict = await _pageEventRepository.GetEventsAppliedPageCount(ids);

                items.ForEach(i =>
                {
                    i.AppliedPages = appliedPageCountDict.TryGetValue(i.Id, out int value) ? value : 0;
                });

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select(c => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public new async Task<Guid> DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("DELETE");
                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("Custom event not found.");
                entity.IsActive = false;
                entity.DeletedDate = DateTimeOffset.UtcNow;
                entity.DeletedBy = "";

                // Deleting the page event data
                var pageEvents = await _pageEventRepository.GetsAsync(query => query.Where(c => c.EventId == id));
                pageEvents.ForEach(pe =>
                {
                    pe.IsActive = false;
                    pe.DeletedDate = DateTimeOffset.UtcNow;
                });
                await _pageEventRepository.UpdateRangeAsync(pageEvents);

                // Deleting the profile page event data
                var profilePageEvents = await _profilePageEventRepository.GetsAsync(query => query.Where(c => c.EventId == id));
                profilePageEvents.ForEach(ppe =>
                {
                    ppe.IsActive = false;
                    ppe.DeletedDate = DateTimeOffset.UtcNow;
                });
                await _profilePageEventRepository.UpdateRangeAsync(profilePageEvents);

                await _repository.UpdateAsync(entity);
                AppendRecords(entity.Id.ToString());
                await _unitOfWork.CommitAsync();
                return entity.Id;
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
