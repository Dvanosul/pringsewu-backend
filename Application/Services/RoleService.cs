
using System.Text.RegularExpressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Services
{
    public class RoleService : BaseCrudService<
        RoleService,
        Context,
        RoleDTO,
        RolePaginationDTO,
        RoleParam,
        Role,
        IRoleRepository>, IRoleService
    {
        private readonly IUserTypeRepository _userTypeRepository;

        public RoleService(
            IConfiguration configuration,
            ILogger<RoleService> logger,
            IUnitOfWork<Context>
            unitOfWork, IRoleRepository repository,
            IUserTypeRepository userTypeRepository
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _userTypeRepository = userTypeRepository;
        }

        public new async Task<Guid> CreateAsync(RoleParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                var userTypeExist = await _userTypeRepository.IsExistAsync(param.UserTypeId);
                if (!userTypeExist)
                {
                    throw new NotFoundException("User type not found.");
                }

                var entity = param.Adapt<Role>();
                Guid id = await _repository.CreateAsync(entity);
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

        public new async Task<RoleDTO> GetAsync(Guid id)
        {
            try
            {
                StartOperation("GET");
                var val = (await _repository.GetAsync(id, q => q.Include(r => r.UserType))) ?? throw new NotFoundException("TEntity not found.");
                var result = val.Adapt<RoleDTO>();
                AppendRecords(val.Id.ToString());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public new async Task<PaginationResponse<RolePaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery, q => q.Include(r => r.UserType));
                var items = itemCountResponse.Items.Adapt<List<RolePaginationDTO>>();
                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select((c) => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public new async Task<Guid> UpdateAsync(RoleParam param, Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                var userTypeExist = await _userTypeRepository.IsExistAsync(param.UserTypeId);
                if (!userTypeExist)
                {
                    throw new NotFoundException("User type not found.");
                }

                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("TEntity not found.");
                param.Adapt(entity);
                entity.UpdatedDate = DateTimeOffset.UtcNow;
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

        public async Task<List<RoleDTO>> GetsByCodeAsync(List<string>? codes = null)
        {
            try
            {
                StartOperation("GET");
                var roles = await _repository.GetsByCodeAsync(codes);
                return roles.Adapt<List<RoleDTO>>();
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<bool> SynchronizeRoles(List<string> rolesToAdd, List<string> rolesToRemove)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPSERT");
                var removeRoles = await _repository.GetsByCodeAsync(rolesToRemove);
                foreach (var role in removeRoles)
                {
                    role.IsActive = false;
                    role.DeletedDate = DateTimeOffset.UtcNow;
                }
                if (removeRoles.Any())
                {
                    await _repository.UpdateRangeAsync(removeRoles);
                }

                var addRoles = new List<Role>();
                var roles = ConvertCodesToPascalCaseDictionary(rolesToAdd);
                foreach (var role in roles)
                {
                    addRoles.Add(new Role
                    {
                        Code = role.Key,
                        Name = role.Value,
                    });
                }
                if (addRoles.Any())
                {
                    await _repository.CreateRangeAsync(addRoles);
                }
                await _unitOfWork.CommitAsync();
                return true;
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

        private string ConvertToPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var words = Regex.Split(input, @"[-_\s]+");

            return string.Join(" ", words.Select(word =>
                char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }

        private Dictionary<string, string> ConvertCodesToPascalCaseDictionary(IEnumerable<string> codes)
        {
            if (codes == null || !codes.Any())
                return new Dictionary<string, string>();

            return codes.ToDictionary(
                code => code,
                code => ConvertToPascalCase(code)
            );
        }
    }
}
