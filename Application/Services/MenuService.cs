using Sindika.AspNet.app015.Application.DTOs.Menu;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Exceptions.BadRequest.StructureFormatError;
using Sindika.AspNet.Exceptions.BadRequest;

namespace Sindika.AspNet.app015.Application.Services
{
    public class MenuService : BaseCrudService<
        MenuService,
        Context,
        MenuDetailDTO,
        MenuPaginationDTO,
        MenuParam,
        Menu,
        IMenuRepository>, IMenuService
    {
        private readonly IRoleProfileRepository _roleProfileRepository;
        private readonly IProfilePageEventRepository _profilePageEventRepository;
        private readonly IMenuPageEventRepository _menuPageEventRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPageEventRepository _pageEventRepository;

        public MenuService(
            IConfiguration configuration,
            ILogger<MenuService> logger,
            IUnitOfWork<Context>
            unitOfWork, IMenuRepository repository,
            IRoleProfileRepository roleProfileRepository,
            IProfilePageEventRepository profilePageEventRepository,
            IMenuPageEventRepository menuPageEventRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IPageEventRepository pageEventRepository
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _roleProfileRepository = roleProfileRepository;
            _profilePageEventRepository = profilePageEventRepository;
            _menuPageEventRepository = menuPageEventRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _pageEventRepository = pageEventRepository;
        }

        public new async Task<MenuDetailDTO> GetAsync(Guid id)
        {
            try
            {
                StartOperation("GET");
                var val = await _repository.GetAsync(id) ?? throw new NotFoundException("Menu not found.");
                var pageEvent = await _menuPageEventRepository.GetByMenuIdAsync(id, q => q.Include(c => c.PageEvent));

                var result = val.Adapt<MenuDetailDTO>();
                result.PageId = pageEvent?.PageEvent?.PageId;
                result.EventId = pageEvent?.PageEvent?.EventId;

                AppendRecords(val.Id.ToString());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task MoveAsync(MoveMenuParam param, Guid id)
        {
            if (param.Index <= 0)
                throw new BadRequestException("ERR-CTM-003", "Menu index must be positive.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                if (id == param.ParentMenu) throw new BadRequestException("ERR-CTM-002", "Cannot move to the same menu.");

                var menu = await _repository.GetAsync(id) ?? throw new NotFoundException("Menu not found.");
                if (menu.Number == "1") throw new BadRequestException("ERR-CTM-002", "Cannot move root menu.");

                // Get the target menu and its children
                var items = await _repository.GetsAsync(q => q.Where(m => m.InternalCode.StartsWith(menu.InternalCode)));
                var itemIds = items.Select(m => m.Id);
                if (itemIds.Contains(param.ParentMenu)) throw new BadRequestException("ERR-CTM-004", "Cannot move menu to its own child.");

                // Shift old siblings up
                var youngerSiblingCount = await AdjustRelatedMenuAsync(menu.InternalCode, -1, itemIds);

                var newParentMenu = await _repository.GetAsync(param.ParentMenu) ?? throw new NotFoundException("Parent menu not found.");
                var newInternalCode = $"{newParentMenu.InternalCode}:{param.Index.ToString().PadLeft(3, '0')}";
                var newNumber = $"{newParentMenu.Number}.{param.Index}";

                if (menu.InternalCode.StartsWith(newParentMenu.InternalCode) && newInternalCode.Length == menu.InternalCode.Length)
                {
                    youngerSiblingCount++;
                }

                // Handle old parent menu collapse if needed
                await HandleOldParentMenuCollapse(menu.Number, youngerSiblingCount);

                if (menu.InternalCode == newInternalCode) throw new BadRequestException("ERR-CTM-004", "No need to move");


                // Shift new siblings down
                await AdjustRelatedMenuAsync(newInternalCode, 1, itemIds);

                // Update the menu and its children
                foreach (var item in items)
                {
                    item.InternalCode = item.InternalCode.Replace(menu.InternalCode, newInternalCode);
                    item.Number = item.Number.Replace(menu.Number, newNumber);
                    item.UpdatedDate = DateTimeOffset.UtcNow;
                }

                // Ensure new parent is expanded
                if (!newParentMenu.IsExpanded)
                {
                    newParentMenu.IsExpanded = true;
                    newParentMenu.UpdatedDate = DateTimeOffset.UtcNow;

                    items.Add(newParentMenu);
                }

                await _repository.UpdateRangeAsync(items);
                AppendRecords(menu.Id.ToString());
                await _unitOfWork.CommitAsync();
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

        private async Task HandleOldParentMenuCollapse(string menuNumber, int youngerSiblingCount)
        {
            if (youngerSiblingCount == 0 && menuNumber.EndsWith(".1"))
            {
                var oldParentNumber = menuNumber[..menuNumber.LastIndexOf('.')];
                var parentMenu = await _repository.GetByIdentifiersAsync(new Dictionary<string, object>
                {
                    { "Number", oldParentNumber }
                }) ?? throw new NotFoundException("Parent menu not found.");

                parentMenu.IsExpanded = false;
                parentMenu.UpdatedDate = DateTimeOffset.UtcNow;

                await _repository.UpdateAsync(parentMenu);
                _unitOfWork.GetContext().ChangeTracker.Clear();
            }
        }

        public new async Task<Guid> CreateAsync(MenuParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                if (!param.ParentMenu.HasValue)
                {
                    throw new MissingRequiredFieldException("Data.ParentMenu");
                }

                var parentMenu = await _repository.GetAsync(param.ParentMenu.Value)
                                 ?? throw new NotFoundException("Parent Menu not found.");

                if (!parentMenu.IsExpanded)
                {
                    parentMenu.IsExpanded = true;
                    await _repository.UpdateAsync(parentMenu);
                }
                var childCount = await _repository.GetMenuChildCountAsync(parentMenu.InternalCode);

                var menu = param.Adapt<Menu>();
                menu.Number = $"{parentMenu.Number}.{childCount + 1}";
                menu.InternalCode = $"{parentMenu.InternalCode}:{(childCount + 1).ToString().PadLeft(3, '0')}";
                Guid id = await _repository.CreateAsync(menu);

                if (param.PageId.HasValue && param.EventId.HasValue)
                {
                    var pageEvent = await _pageEventRepository.GetByIdentifiersAsync(new Dictionary<string, object>
                        {
                            { "PageId", param.PageId!.Value },
                            { "EventId", param.EventId!.Value }
                        }) ?? throw new NotFoundException("Page event not found.");
                    var menuPageEvent = new MenuPageEvent
                    {
                        MenuId = id,
                        PageEventId = pageEvent.Id
                    };
                    await _menuPageEventRepository.CreateAsync(menuPageEvent);
                }

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

        public new async Task<Guid> UpdateAsync(MenuParam param, Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                var menu = await _repository.GetAsync(id) ?? throw new NotFoundException("Menu not found.");
                param.Adapt(menu);
                menu.UpdatedDate = DateTimeOffset.UtcNow;
                await _repository.UpdateAsync(menu);

                var menuPageEvent = await _menuPageEventRepository.GetByMenuIdAsync(id);
                var havePageEvent = param.PageId.HasValue && param.EventId.HasValue;
                if (menuPageEvent != null)
                {
                    if (havePageEvent)
                    {
                        var pageEvent = await _pageEventRepository.GetByIdentifiersAsync(new Dictionary<string, object>
                        {
                            { "PageId", param.PageId!.Value },
                            { "EventId", param.EventId!.Value }
                        }) ?? throw new NotFoundException("Page event not found.");
                        menuPageEvent.PageEventId = pageEvent.Id;
                        menuPageEvent.UpdatedDate = DateTimeOffset.UtcNow;

                        await _menuPageEventRepository.UpdateAsync(menuPageEvent);
                    }
                    else
                    {
                        menuPageEvent.IsActive = false;
                        menuPageEvent.DeletedDate = DateTimeOffset.UtcNow;

                        await _menuPageEventRepository.UpdateAsync(menuPageEvent);
                    }
                }
                else if (havePageEvent)
                {
                    var pageEvent = await _pageEventRepository.GetByIdentifiersAsync(new Dictionary<string, object>
                        {
                            { "PageId", param.PageId!.Value },
                            { "EventId", param.EventId!.Value }
                        }) ?? throw new NotFoundException("Page event not found.");

                    menuPageEvent = new MenuPageEvent
                    {
                        MenuId = id,
                        PageEventId = pageEvent.Id
                    };
                    await _menuPageEventRepository.CreateAsync(menuPageEvent);
                }

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

        public new async Task<Guid> DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("DELETE");

                var menu = (await _repository.GetAsync(id)) ?? throw new NotFoundException("Menu not found.");

                if (menu.Number == "1") throw new BadRequestException("ERR-CTM-002", "Cannot delete root menu.");
                if (menu.IsExpanded) throw new BadRequestException("ERR-CTM-002", "Cannot delete menu with child.");

                menu.IsActive = false;
                menu.DeletedDate = DateTimeOffset.UtcNow;
                menu.DeletedBy = "";
                await _repository.UpdateAsync(menu);

                var menuPageEvent = await _menuPageEventRepository.GetByMenuIdAsync(id);
                if (menuPageEvent != null)
                {
                    menuPageEvent.IsActive = false;
                    menuPageEvent.DeletedDate = DateTimeOffset.UtcNow;
                    menuPageEvent.DeletedBy = "";
                    await _menuPageEventRepository.UpdateAsync(menuPageEvent);
                }

                var youngerSiblingCount = await AdjustRelatedMenuAsync(menu.InternalCode, -1, []);

                await HandleOldParentMenuCollapse(menu.Number, youngerSiblingCount);

                AppendRecords(menu.Id.ToString());
                await _unitOfWork.CommitAsync();
                return menu.Id;
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

        public async Task<List<MenuDTO>> GetUserMenuListAsync(Guid userId, string roleActive)
        {
            try
            {
                StartOperation("GET");

                return await GetMenuListAsync(userId, roleActive);
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<List<MenuDTO>> GetAllMenuListAsync()
        {
            try
            {
                StartOperation("GET");

                return await GetMenuListAsync();
            }
            finally
            {
                EndOperation();
            }
        }

        private async Task<int> AdjustRelatedMenuAsync(string internalCode, int offset, IEnumerable<Guid> blacklist)
        {
            var level = internalCode.Count(c => c == ':');
            var parentInternalCode = internalCode[..^3];

            var menuYoungerSiblings = await _repository.GetsAsync(q =>
            {

                return q.Where(m => m.InternalCode.StartsWith(parentInternalCode)
                                    && !blacklist.Contains(m.Id)
                                    && m.InternalCode.CompareTo(internalCode) >= 0);
            });

            foreach (var sibling in menuYoungerSiblings)
            {
                var internalCodes = sibling.InternalCode.Split(':');
                var numbers = sibling.Number.Split('.');

                var index = int.Parse(internalCodes[level]) + offset;
                internalCodes[level] = index.ToString().PadLeft(3, '0');
                numbers[level] = index.ToString();

                sibling.InternalCode = string.Join(":", internalCodes);
                sibling.Number = string.Join(".", numbers);
                sibling.UpdatedDate = DateTimeOffset.UtcNow;
            }

            await _repository.UpdateRangeAsync(menuYoungerSiblings);
            _unitOfWork.GetContext().ChangeTracker.Clear();
            return menuYoungerSiblings.Count;
        }

        private async Task PopulateMenuPageEventAsync(List<MenuDTO> menus)
        {
            var menuIds = menus.Select(m => m.Id);
            var menuPageEvents = await _menuPageEventRepository.GetsAsync(q =>
            {
                return q.Where(mpe => menuIds.Contains(mpe.MenuId))
                        .Select(mpe => new { mpe.MenuId, PageName = mpe.PageEvent!.Page!.Name, EventName = mpe.PageEvent!.Event!.Name });
            });
            var menuPageEventsDict = menuPageEvents.ToDictionary(mpe => mpe.MenuId, mpe => new { mpe.PageName, mpe.EventName });

            foreach (var menu in menus)
            {
                var pageEvent = menuPageEventsDict.GetValueOrDefault(menu.Id);

                menu.PageName = pageEvent?.PageName;
                menu.EventName = pageEvent?.EventName;
            }
        }

        private async Task<List<MenuDTO>> GetMenuListAsync(Guid? userId = null, string? roleActive = null)
        {
            var menus = await _repository.GetsAsync(q => q.ProjectToType<MenuDTO>().OrderBy(m => m.InternalCode));
            var allAllowedInternalCodes = new HashSet<string>();

            if (userId.HasValue && !string.IsNullOrEmpty(roleActive))
            {
                var role = await _roleRepository.GetByCodeAsync(roleActive) ?? throw new NotFoundException("Role not found.");
                var profileIds = await _roleProfileRepository.GetUserProfileIdsAsync(userId.Value, role.Id);

                // Get the required page event for each menu
                var menuPageEvents = await _menuPageEventRepository.GetsAsync(q => q.Include(c => c.PageEvent));
                var menuPageEventsDict = menuPageEvents.ToDictionary(mpe => mpe.MenuId, mpe => new { mpe.PageEvent!.PageId, mpe.PageEvent!.EventId });
                var pageIds = menuPageEventsDict.Values.Select(c => c.PageId).ToHashSet();
                var eventIds = menuPageEventsDict.Values.Select(c => c.EventId).ToHashSet();

                // Get all page events that the user have access to
                var profilePageEvents = await _profilePageEventRepository.GetsAsync(q =>
                {
                    return q.Where(ppe => profileIds.Contains(ppe.ProfileId))
                            .Where(ppe => pageIds.Contains(ppe.PageId))
                            .Where(ppe => eventIds.Contains(ppe.EventId));
                });
                var allowedPageEvents = profilePageEvents.Select(ppe => new { ppe.PageId, ppe.EventId }).ToHashSet();

                var allowedMenuIds = menuPageEventsDict.Where(c => allowedPageEvents.Contains(c.Value)).Select(c => c.Key).ToList();
                var allowedInternalCodes = menus.Where(m => allowedMenuIds.Contains(m.Id)).Select(m => m.InternalCode).ToHashSet();
                allAllowedInternalCodes = [.. allowedInternalCodes];

                foreach (var internalCode in allowedInternalCodes)
                {
                    var parts = internalCode.Split(':');
                    if (parts.Length <= 1) continue; // top-level menu, no parent

                    // Collect all parent internal codes
                    for (int i = 1; i < parts.Length; i++)
                    {
                        var parentInternalCode = string.Join(":", parts.Take(i));
                        allAllowedInternalCodes.Add(parentInternalCode); // add parent
                    }
                }

                menus = menus
                    .Where(m => allAllowedInternalCodes.Contains(m.InternalCode))
                    .ToList();
            }
            else
            {
                await PopulateMenuPageEventAsync(menus);
            }

            AppendRecords(null, menus.Select(m => m.Id.ToString()).ToList());
            return BuildMenuTree(menus);
        }

        private List<MenuDTO> BuildMenuTree(IEnumerable<MenuDTO> menus)
        {
            var internalCodeDict = menus.ToDictionary(m => m.InternalCode);
            List<MenuDTO> rootMenus = [];

            foreach (var menu in internalCodeDict.Values)
            {
                var parts = menu.InternalCode.Split(':');
                if (parts.Length > 1)
                {
                    // Find parent by InternalCode
                    var parentInternalCode = string.Join(":", parts.Take(parts.Length - 1));
                    if (internalCodeDict.TryGetValue(parentInternalCode, out var parentMenu))
                    {
                        parentMenu.Children ??= [];
                        parentMenu.Children.Add(menu);
                    }
                }
                else
                {
                    // Top-level menu (no parent)
                    rootMenus.Add(menu);
                }
            }

            return rootMenus;
        }
    }
}
