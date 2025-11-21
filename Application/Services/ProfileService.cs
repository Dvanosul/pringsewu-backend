using Mapster;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.Profile;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Authentication.Interfaces;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Exceptions.NotFound;

namespace Sindika.AspNet.app015.Application.Services
{
    public class ProfileService : BaseCrudService<
        ProfileService,
        Context,
        ProfileDTO,
        ProfilePaginationDTO,
        ProfileParam,
        Profile,
        IProfileRepository>, IProfileService
    {
        private readonly IProfilePageEventRepository _profilePageEventRepository;
        private readonly IRoleProfileRepository _roleProfileRepository;
        private readonly ISynchronizeCache _synchronizeCache;

        public ProfileService(
            IConfiguration configuration,
            ILogger<ProfileService> logger,
            IUnitOfWork<Context>
            unitOfWork, IProfileRepository repository,
            IProfilePageEventRepository profilePageEventRepository,
            IRoleProfileRepository roleProfileRepository,
            ISynchronizeCache synchronizeCache
            ) : base(configuration, logger, unitOfWork, repository)
        {
            _profilePageEventRepository = profilePageEventRepository;
            _roleProfileRepository = roleProfileRepository;
            _synchronizeCache = synchronizeCache;
        }

        public async Task<Guid> DuplicateAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");
                var param = (await _repository.GetAsync(id))?.Adapt<ProfileParam>() ?? throw new NotFoundException("Profile not found.");

                param.Code += "-copy";
                param.Name += " copy";

                var newId = await _repository.CreateAsync(param.Adapt<Profile>());
                var profilePageEvents = await _profilePageEventRepository.GetsAsync(q => q.Where(ppe => ppe.ProfileId == id));

                profilePageEvents.ForEach(ppe =>
                {
                    ppe.Id = Guid.Empty;
                    ppe.ProfileId = newId;
                    ppe.CreatedDate = DateTimeOffset.UtcNow;
                    ppe.UpdatedDate = null;
                    ppe.UpdatedBy = null;
                });

                await _profilePageEventRepository.CreateRangeAsync(profilePageEvents);
                AppendRecords(newId.ToString());
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
                var entity = (await _repository.GetAsync(id)) ?? throw new NotFoundException("Profile not found.");
                entity.IsActive = false;
                entity.DeletedDate = DateTimeOffset.UtcNow;
                entity.DeletedBy = "";
                await _repository.UpdateAsync(entity);

                var profilePageEvents = await _profilePageEventRepository.GetsAsync(q => q.Where(ppe => ppe.ProfileId == id));
                profilePageEvents.ForEach(ppe =>
                {
                    ppe.IsActive = false;
                    ppe.UpdatedDate = DateTimeOffset.UtcNow;
                });

                await _profilePageEventRepository.UpdateRangeAsync(profilePageEvents);

                AppendRecords(entity.Id.ToString());
                await _unitOfWork.CommitAsync();

                var roles = await _roleProfileRepository.GetsAsync(q => q.Where(rp => rp.ProfileId == id).Include(rp => rp.Role));
                var roleCodes = roles.Select(c => c.Role!.Code).ToHashSet();

                await _synchronizeCache.ClearKeys(roleCodes);
                await _synchronizeCache.GenerateRoleKeys(roleCodes);

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
