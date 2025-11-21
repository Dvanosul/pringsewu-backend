using Mapster;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.RoleProfile;
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

namespace Sindika.AspNet.app015.Application.Services
{
    public class RoleProfileService : BaseCrudService<
        RoleProfileService,
        Context,
        RoleProfileDTO,
        RoleProfilePaginationDTO,
        RoleProfileParam,
        RoleProfile,
        IRoleProfileRepository>, IRoleProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISynchronizeCache _synchronizeCache;

        public RoleProfileService(
            IConfiguration configuration,
            ILogger<RoleProfileService> logger,
            IUnitOfWork<Context>
            unitOfWork, IRoleProfileRepository repository,
            IRoleRepository roleRepository,
            IProfileRepository profileRepository,
            ISynchronizeCache synchronizeCache
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _profileRepository = profileRepository;
            _roleRepository = roleRepository;
            _synchronizeCache = synchronizeCache;
        }

        public async Task<PaginationResponse<RoleProfilePaginationDTO>> GetCustomPaginationAsync(Guid roleId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _profileRepository.GetPaginationAsync(paginationQuery);
                var items = itemCountResponse.Items.Adapt<List<RoleProfilePaginationDTO>>();
                var ids = itemCountResponse.Items.Select((c) => c.Id).ToList();

                var profiles = await _repository.GetsAsync(query => query.Where(rp => rp.RoleId == roleId && ids.Contains(rp.ProfileId)));
                var profilesId = profiles.Select(c => c.ProfileId).ToHashSet();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items.Select(p =>
                {
                    p.IsEnabled = profilesId.Contains(p.Id);
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

        public async Task UpsertAsync(RoleProfileParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPSERT");
                var role = await _roleRepository.GetAsync(param.RoleId) ?? throw new NotFoundException("Role not found.");

                if (!await _profileRepository.IsAllExistAsync(param.ChangedProfiles))
                {
                    throw new NotFoundException("Profile not found");
                }

                var records = await _repository.UpsertAsync(param.RoleId, param.ChangedProfiles);
                AppendRecords(null, records.Adapt<List<string>>());
                await _unitOfWork.CommitAsync();
                await _synchronizeCache.ClearKeys([role.Code]);
                await _synchronizeCache.GenerateRoleKeys([role.Code]);
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
