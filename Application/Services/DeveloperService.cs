using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Application.DTOs.Developer;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Mapster;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Exceptions.BadRequest.StructureFormatError;
using Sindika.AspNet.Exceptions.BadRequest;

namespace Sindika.AspNet.app015.Application.Services
{
    public class DeveloperService : BaseCrudService<
        DeveloperService,
        Context,
        DeveloperDTO,
        DeveloperPaginationDTO,
        CreateDeveloperParam,
        Developer,
        IDeveloperRepository>, IDeveloperService
    {
        private readonly IDeveloperUserTypeRepository _developerUserTypeRepository;
        private readonly IRoleRepository _roleRepository;

        public DeveloperService(
            IConfiguration configuration,
            ILogger<DeveloperService> logger,
            IUnitOfWork<Context>
            unitOfWork, IDeveloperRepository repository,
            IDeveloperUserTypeRepository developerUserTypeRepository,
            IRoleRepository roleRepository
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _developerUserTypeRepository = developerUserTypeRepository;
            _roleRepository = roleRepository;
        }

        public new async Task<Guid> CreateAsync(CreateDeveloperParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                if (!param.Roles.Any()) throw new MissingRequiredFieldException("roles");

                if (!await _roleRepository.IsAllExistAsync(param.Roles))
                {
                    throw new NotFoundException("Role not found");
                }

                var entity = param.Adapt<Developer>();
                Guid id = await _repository.CreateAsync(entity);
                var userTypeId = await _developerUserTypeRepository.GetUserTypeIdAsync();

                var roles = param.Roles.Select(roleId => new DeveloperUserType
                {
                    DeveloperId = id,
                    UserTypeId = userTypeId,
                    RoleId = roleId
                }).ToList();
                await _developerUserTypeRepository.CreateRangeAsync(roles);

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

                if (await _developerUserTypeRepository.IsAttachedAsync(id))
                {
                    throw new BadRequestException("ERR-CTM-001", "This Developer is attached to a User");
                }

                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("Developer not found.");
                entity.IsActive = false;
                entity.DeletedDate = DateTimeOffset.UtcNow;
                entity.DeletedBy = "";
                await _repository.UpdateAsync(entity);

                // delete relation to role and user
                var relations = await _developerUserTypeRepository.GetsAsync(q => q.Where(_developerUserTypeRepository.IdentifierFilter(id)));
                relations.ForEach(r =>
                {
                    r.IsActive = false;
                    r.DeletedDate = DateTimeOffset.UtcNow;
                    r.DeletedBy = "";
                });
                await _developerUserTypeRepository.UpdateRangeAsync(relations);

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

        public async Task<Guid> UpdateAsync(UpdateDeveloperParam param, Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                if (!await _roleRepository.IsAllExistAsync(param.ChangedRoles))
                {
                    throw new NotFoundException("Role not found");
                }

                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("Developer not found.");
                param.Adapt(entity);
                entity.UpdatedDate = DateTimeOffset.UtcNow;
                await _repository.UpdateAsync(entity);

                var existingRoles = await _developerUserTypeRepository.GetsAsync(q => q.Where(_developerUserTypeRepository.IdentifierFilter(id))
                                                                                       .Where(eut => param.ChangedRoles.Contains(eut.RoleId!.Value)));
                var rolesToAdd = param.ChangedRoles.Except(existingRoles.Select(r => r.RoleId!.Value));


                existingRoles.ForEach(eut =>
                {
                    eut.IsActive = false;
                    eut.DeletedDate = DateTimeOffset.UtcNow;
                    eut.DeletedBy = "";
                });

                if (rolesToAdd.Any())
                {
                    var userTypeId = await _developerUserTypeRepository.GetUserTypeIdAsync();
                    var userId = await _developerUserTypeRepository.GetUserIdAsync(id);

                    await _developerUserTypeRepository.CreateRangeAsync(
                        rolesToAdd.Select(roleId => new DeveloperUserType
                        {
                            DeveloperId = id,
                            UserTypeId = userTypeId,
                            RoleId = roleId,
                            UserId = userId
                        }).ToList()
                    );
                }

                if (existingRoles.Count != 0)
                {
                    await _developerUserTypeRepository.UpdateRangeAsync(existingRoles);
                }

                if (!await _developerUserTypeRepository.IsExistAsync(id))
                {
                    throw new BadRequestException("VAL-GEN-002", "Needs to have at least one role");
                }

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

        public async Task<PaginationResponse<DeveloperPaginationDTO>> GetAvailablePaginationAsync(PaginationQuery paginationQuery, Guid? userId = null)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetAvailablePaginationAsync(paginationQuery, userId);
                var items = itemCountResponse.Items.Adapt<List<DeveloperPaginationDTO>>();
                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, items.Select(i => i.Id).Adapt<List<string>>());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }


        public new async Task<PaginationResponse<DeveloperPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery);
                var ids = itemCountResponse.Items.Select(i => i.Id);
                var rolesDict = await _developerUserTypeRepository.GetRoleCountsAsync(ids);

                var items = itemCountResponse.Items.Select(i =>
                {
                    var item = i.Adapt<DeveloperPaginationDTO>();
                    item.Roles = rolesDict.GetValueOrDefault(i.Id, 0);

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
    }
}
