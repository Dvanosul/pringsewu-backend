using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Application.DTOs.Employee;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Mapster;
using Sindika.AspNet.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.Exceptions.BadRequest;

namespace Sindika.AspNet.app015.Application.Services
{
    public class EmployeeService : BaseCrudService<
        EmployeeService,
        Context,
        EmployeeDTO,
        EmployeePaginationDTO,
        CreateEmployeeParam,
        Employee,
        IEmployeeRepository>, IEmployeeService
    {
        private readonly IEmployeeUserTypeRepository _employeeUserTypeRepository;
        private readonly IRoleRepository _roleRepository;

        public EmployeeService(
            IConfiguration configuration,
            ILogger<EmployeeService> logger,
            IUnitOfWork<Context>
            unitOfWork, IEmployeeRepository repository,
            IEmployeeUserTypeRepository employeeUserTypeRepository,
            IRoleRepository roleRepository
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _employeeUserTypeRepository = employeeUserTypeRepository;
            _roleRepository = roleRepository;
        }

        public new async Task<Guid> CreateAsync(CreateEmployeeParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                if (!await _roleRepository.IsAllExistAsync(param.Roles))
                {
                    throw new NotFoundException("Role not found");
                }

                var entity = param.Adapt<Employee>();
                Guid id = await _repository.CreateAsync(entity);
                var userTypeId = await _employeeUserTypeRepository.GetUserTypeIdAsync();

                var roles = param.Roles.Select(roleId => new EmployeeUserType
                {
                    EmployeeId = id,
                    UserTypeId = userTypeId,
                    RoleId = roleId
                }).ToList();
                await _employeeUserTypeRepository.CreateRangeAsync(roles);

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

                if (await _employeeUserTypeRepository.IsAttachedAsync(id))
                {
                    throw new BadRequestException("ERR-CTM-001", "This Employee is attached to a User");
                }

                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("Employee not found.");
                entity.IsActive = false;
                entity.DeletedDate = DateTimeOffset.UtcNow;
                entity.DeletedBy = "";
                await _repository.UpdateAsync(entity);

                // delete relation to role and user
                var relations = await _employeeUserTypeRepository.GetsAsync(q => q.Where(_employeeUserTypeRepository.IdentifierFilter(id)));
                relations.ForEach(r =>
                {
                    entity.IsActive = false;
                    entity.DeletedDate = DateTimeOffset.UtcNow;
                    entity.DeletedBy = "";
                });
                await _employeeUserTypeRepository.UpdateRangeAsync(relations);

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

        public async Task<Guid> UpdateAsync(UpdateEmployeeParam param, Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                if (!await _roleRepository.IsAllExistAsync(param.ChangedRoles))
                {
                    throw new NotFoundException("Role not found");
                }

                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("Employee not found.");
                param.Adapt(entity);
                entity.UpdatedDate = DateTimeOffset.UtcNow;
                await _repository.UpdateAsync(entity);

                var existingRoles = await _employeeUserTypeRepository.GetsAsync(q => q.Where(_employeeUserTypeRepository.IdentifierFilter(id))
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
                    var userTypeId = await _employeeUserTypeRepository.GetUserTypeIdAsync();
                    var userId = await _employeeUserTypeRepository.GetUserIdAsync(id);

                    await _employeeUserTypeRepository.CreateRangeAsync(
                        rolesToAdd.Select(roleId => new EmployeeUserType
                        {
                            EmployeeId = id,
                            UserTypeId = userTypeId,
                            RoleId = roleId,
                            UserId = userId
                        }).ToList()
                    );
                }

                if (existingRoles.Count != 0)
                {
                    await _employeeUserTypeRepository.UpdateRangeAsync(existingRoles);
                }

                if (!await _employeeUserTypeRepository.IsExistAsync(id))
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

        public new async Task<PaginationResponse<EmployeePaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery);
                var ids = itemCountResponse.Items.Select(i => i.Id);
                var rolesDict = await _employeeUserTypeRepository.GetRoleCountsAsync(ids);

                var items = itemCountResponse.Items.Select(i =>
                {
                    var item = i.Adapt<EmployeePaginationDTO>();
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

        public async Task<PaginationResponse<EmployeePaginationDTO>> GetAvailablePaginationAsync(PaginationQuery paginationQuery, Guid? userId = null)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetAvailablePaginationAsync(paginationQuery, userId);
                var items = itemCountResponse.Items.Adapt<List<EmployeePaginationDTO>>();
                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, items.Select(i => i.Id).Adapt<List<string>>());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }
    }
}
