using Sindika.AspNet.app015.Application.DTOs.User;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Mapster;
using Sindika.AspNet.Exceptions.BadRequest.StructureFormatError;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Sindika.AspNet.Exceptions.BadRequest;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.Developer;
using Sindika.AspNet.app015.Application.DTOs.Employee;

namespace Sindika.AspNet.app015.Application.Services
{
    public class UserService : BaseCrudService<
        UserService,
        Context,
        UserDTO,
        UserPaginationDTO,
        CreateUserParam,
        User,
        IUserRepository>, IUserService
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserUserTypeRepository _userUserTypeRepository;
        private readonly IDeveloperUserTypeRepository _developerUserTypeRepository;
        private readonly IEmployeeUserTypeRepository _employeeUserTypeRepository;
        private readonly IRoleProfileRepository _roleProfileRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IPageEventRepository _pageEventRepository;
        private readonly IProfilePageEventRepository _profilePageEventRepository;

        public UserService(
            IConfiguration configuration,
            ILogger<UserService> logger,
            IUnitOfWork<Context>
            unitOfWork, IUserRepository repository,
            IDeveloperRepository developerRepository,
            IEmployeeRepository employeeRepository,
            IUserUserTypeRepository userUserTypeRepository,
            IDeveloperUserTypeRepository developerUserTypeRepository,
            IEmployeeUserTypeRepository employeeUserTypeRepository,
            IRoleProfileRepository roleProfileRepository,
            IPageRepository pageRepository,
            IPageEventRepository pageEventRepository,
            IProfilePageEventRepository profilePageEventRepository
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _developerRepository = developerRepository;
            _employeeRepository = employeeRepository;
            _userUserTypeRepository = userUserTypeRepository;
            _developerUserTypeRepository = developerUserTypeRepository;
            _employeeUserTypeRepository = employeeUserTypeRepository;
            _roleProfileRepository = roleProfileRepository;
            _pageRepository = pageRepository;
            _pageEventRepository = pageEventRepository;
            _profilePageEventRepository = profilePageEventRepository;
        }

        async Task AssignDeveloperToUser(Guid developerId, Guid userId)
        {
            if (!await _developerRepository.IsExistAsync(developerId))
            {
                throw new NotFoundException("Developer not found");
            }

            if (!await _developerUserTypeRepository.AssignUserAsync(userId, developerId))
            {
                throw new NotFoundException("Developer not available");
            }
        }

        async Task AssignEmployeeToUser(Guid employeeId, Guid userId)
        {
            if (!await _employeeRepository.IsExistAsync(employeeId))
            {
                throw new NotFoundException("Employee not found");
            }

            if (!await _employeeUserTypeRepository.AssignUserAsync(userId, employeeId))
            {
                throw new NotFoundException("Employee not available");
            }
        }

        public new async Task<UserDTO> GetAsync(Guid id)
        {
            try
            {
                StartOperation("GET");
                var val = (await _repository.GetAsync(id, q => q.Include(u => u.Language).Include(u => u.Zone)))
                          ?? throw new NotFoundException("User not found.");
                var result = val.Adapt<UserDTO>();

                result.Developer = (await _developerUserTypeRepository.GetByUserIdAsync(id))?.Developer.Adapt<DeveloperDTO>();
                result.Employee = (await _employeeUserTypeRepository.GetByUserIdAsync(id))?.Employee.Adapt<EmployeeDTO>();

                AppendRecords(val.Id.ToString());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public new async Task<Guid> CreateAsync(CreateUserParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                if (!param.DeveloperId.HasValue && !param.EmployeeId.HasValue)
                {
                    throw new MissingRequiredFieldException("developerId | employeeId");
                }

                if (!await _repository.IsEmailUniqueAsync(param.Email))
                {
                    throw new BadRequestException("VAL-GEN-005", "This email already exists. It must be unique.");
                }

                var entity = param.Adapt<User>();
                entity.Username = Regex.Replace(entity.Name.ToLower().Trim(), @"[^a-z0-9]", "");
                var id = await _repository.CreateAsync(entity);

                if (param.DeveloperId.HasValue)
                {
                    await AssignDeveloperToUser(param.DeveloperId.Value, id);
                }

                if (param.EmployeeId.HasValue)
                {
                    await AssignEmployeeToUser(param.EmployeeId.Value, id);
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
                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("User not found.");
                entity.IsActive = false;
                entity.DeletedDate = DateTimeOffset.UtcNow;
                entity.DeletedBy = "";

                var userUserTypes = await _userUserTypeRepository.GetsAsync(q => q.Where(uut => uut.UserId == id));

                if (userUserTypes.Count != 0)
                {
                    userUserTypes.ForEach(uut =>
                    {
                        uut.UserId = null;
                        uut.UpdatedDate = DateTimeOffset.UtcNow;
                    });

                    await _userUserTypeRepository.UpdateRangeAsync(userUserTypes);
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

        public new async Task<PaginationResponse<UserPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery, q => q.Include(u => u.Language).Include(u => u.Zone));
                var items = itemCountResponse.Items.Adapt<List<UserPaginationDTO>>();
                var userIds = items.Select(i => i.Id).ToHashSet();

                var userTypes = await _userUserTypeRepository.GetUsersUserTypesAsync(userIds);

                items.ForEach(i =>
                {
                    i.UserTypes = userTypes.GetValueOrDefault(i.Id, []);
                });

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select((c) => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public new async Task<Guid> UpdateAsync(CreateUserParam param, Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                if (!param.DeveloperId.HasValue && !param.EmployeeId.HasValue)
                {
                    throw new MissingRequiredFieldException("developerId | employeeId");
                }

                if (!await _repository.IsEmailUniqueAsync(param.Email, id))
                {
                    throw new BadRequestException("VAL-GEN-005", "This email already exists. It must be unique.");
                }


                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("User not found.");
                param.Adapt(entity);
                entity.Username = Regex.Replace(entity.Name.ToLower().Trim(), @"[^a-z0-9]", "");
                entity.UpdatedDate = DateTimeOffset.UtcNow;

                var currentDeveloper = await _developerUserTypeRepository.GetByUserIdAsync(id);
                var currentEmployee = await _employeeUserTypeRepository.GetByUserIdAsync(id);

                // If the developer value is changed
                if (currentDeveloper?.DeveloperId != param.DeveloperId)
                {
                    // Delete the old one if any
                    if (currentDeveloper != null)
                    {
                        await _developerUserTypeRepository.UnassignUserAsync(id);
                    }

                    // Insert the new one if any
                    if (param.DeveloperId.HasValue)
                    {
                        await AssignDeveloperToUser(param.DeveloperId.Value, id);
                    }
                }

                // If the employee value is changed
                if (currentEmployee?.EmployeeId != param.EmployeeId)
                {
                    // Delete the old one if any
                    if (currentEmployee != null)
                    {
                        await _employeeUserTypeRepository.UnassignUserAsync(id);
                    }

                    // Insert the new one if any
                    if (param.EmployeeId.HasValue)
                    {
                        await AssignEmployeeToUser(param.EmployeeId.Value, id);
                    }
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

        public async Task<PaginationResponse<PageWithEventsIdPaginationDTO>> GetUserPageAccessPaginationAsync(Guid userId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                var itemCountResponse = await _pageRepository.GetPaginationAsync(paginationQuery);
                var ids = itemCountResponse.Items.Select(i => i.Id).ToList();

                var userProfiles = await _roleProfileRepository.GetUserProfileIdsAsync(userId);
                var customEventsDict = await _pageEventRepository.GetPagesCustomEventCount(ids);
                var pageEventsDict = await _profilePageEventRepository.GetPageAccessAndCustomEventCountByProfilesAsync(userProfiles.ToList(), ids);

                var items = itemCountResponse.Items.Select(i =>
                {
                    var item = i.Adapt<PageWithEventsIdPaginationDTO>();
                    var (events, customEventsCount) = pageEventsDict.GetValueOrDefault(i.Id, (new HashSet<Guid>(), 0));
                    item.Events = events;
                    item.EnabledCustomEventsCount = customEventsCount;
                    item.CustomEventsCount = customEventsDict.GetValueOrDefault(i.Id, 0);

                    return item;
                }).ToList();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, ids.Adapt<List<string>>());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<PaginationResponse<PageCustomEventDTO>> GetUserPageCustomEventAccessPaginationAsync(Guid userId, Guid pageId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                var itemCountResponse = await _pageEventRepository.GetCustomEventPaginationAsync(pageId, paginationQuery);
                var ids = itemCountResponse.Items.Select(i => i.Id);

                var userProfiles = await _roleProfileRepository.GetUserProfileIdsAsync(userId);
                var pageEventsDict = await _profilePageEventRepository.GetPageEnabledCustomEventIdsAsync(userProfiles, pageId, ids);

                var items = itemCountResponse.Items.Select(i =>
                {
                    var item = i.Adapt<PageCustomEventDTO>();
                    item.IsEnabled = pageEventsDict.Contains(i.Id);

                    return item;
                }).ToList();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, ids.Adapt<List<string>>());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<Dictionary<string, IEnumerable<RoleWithProfilesDTO>>> GetUserTypesRolesAndProfilesAsync(Guid userId)
        {
            try
            {
                StartOperation("GET");

                var data = await _userUserTypeRepository.GetUserTypesRolesAsync(userId);
                var roleProfilesDict = await _roleProfileRepository.GetRolesProfilesAsync(data.Values.SelectMany(c => c.Select(r => r.Id)));

                return data.ToDictionary(c => c.Key, c =>
                {
                    var item = c.Value.Select(r =>
                    {
                        var roleItem = r.Adapt<RoleWithProfilesDTO>();
                        roleItem.Profiles = roleProfilesDict.GetValueOrDefault(r.Id, []);

                        return roleItem;
                    });

                    return item;
                });
            }
            finally
            {
                EndOperation();
            }

        }
    }
}
