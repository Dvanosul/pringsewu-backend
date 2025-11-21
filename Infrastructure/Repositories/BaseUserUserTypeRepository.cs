using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public abstract class BaseUserUserTypeRepository<TEntity, TPEntity, TContext> : BaseRepository<TEntity, TContext>, IBaseUserUserTypeRepository<TEntity, TPEntity> where TEntity : UserUserType where TPEntity : class where TContext : DbContext
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IRoleRepository _roleRepository;

        protected BaseUserUserTypeRepository(IUnitOfWork<TContext> unitOfWork, IUserTypeRepository userTypeRepository, IRoleRepository roleRepository) : base(unitOfWork)
        {
            _userTypeRepository = userTypeRepository;
            _roleRepository = roleRepository;
        }

        public string GetDiscriminator()
        {
            return typeof(TEntity).Name;
        }

        public async Task<Guid> GetUserTypeIdAsync()
        {
            var userType = await _userTypeRepository.GetByCodeAsync(GetDiscriminator());

            return userType!.Id;
        }

        public abstract Expression<Func<TEntity, Guid?>> GetIdentifier();
        public abstract Expression<Func<TEntity, bool>> IdentifierFilter(Guid identifier);
        public abstract Expression<Func<TEntity, bool>> IdentifierContainsFilter(IEnumerable<Guid> identifiers);
        public abstract Expression<Func<TEntity, TPEntity>> IncludeEntity();
        public abstract Expression<Func<TPEntity, bool>> IsAvailableFilter(Guid? userId);
        public Expression<Func<TEntity, bool>> RoleAvailableFilter(Guid identifier)
        {
            return r => r.UserType!.Code == GetDiscriminator() && !_dbSet.Where(uut => uut.IsActive)
                                           .Where(IdentifierFilter(identifier))
                                           .Select(uut => uut.RoleId)
                                           .Contains(r.Id);
        }
        public abstract void SetIdentifier(TEntity entity, Guid identifier);

        public async Task<Guid?> GetUserIdAsync(Guid identifier)
        {
            return await _dbSet.Where(c => c.IsActive && c.UserId.HasValue)
                               .Where(IdentifierFilter(identifier))
                               .Select(c => c.UserId)
                               .FirstOrDefaultAsync();
        }

        public new async Task<bool> IsExistAsync(Guid identifier)
        {
            return await _dbSet.Where(c => c.IsActive)
                               .Where(IdentifierFilter(identifier))
                               .AnyAsync();
        }

        public async Task<bool> IsAttachedAsync(Guid identifier)
        {
            return await _dbSet.Where(c => c.IsActive && c.UserId != null)
                               .Where(IdentifierFilter(identifier))
                               .AnyAsync();
        }

        public async Task<TEntity?> GetAsync(Guid identifier, Guid roleId, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            var source = _dbSet.AsQueryable();
            source = source.Where((c) => c.IsActive && c.RoleId == roleId).Where(IdentifierFilter(identifier));
            if (include != null)
            {
                source = include(source);
            }

            return await source.FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistAsync(Guid identifier, Guid roleId)
        {
            return await _dbSet.Where(c => c.IsActive && c.RoleId == roleId).AnyAsync(IdentifierFilter(identifier));
        }

        public async Task<bool> IsExistAsync(Guid identifier, string roleCode)
        {
            return await _dbSet.Where(c => c.IsActive && c.Role!.Code == roleCode).AnyAsync(IdentifierFilter(identifier));
        }

        public async Task<TEntity?> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet.Where(c => c.IsActive && c.UserId == userId)
                .Include(IncludeEntity())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AssignUserAsync(Guid userId, Guid identifier)
        {
            var data = await GetsAsync(q => q.Where(c => c.UserId == null).Where(IdentifierFilter(identifier)));

            if (data.Count == 0)
            {
                return false;
            }

            data.ForEach(dut =>
            {
                dut.UserId = userId;
                dut.UpdatedDate = DateTimeOffset.UtcNow;
            });

            await UpdateRangeAsync(data);
            return true;
        }

        public async Task UnassignUserAsync(Guid userId)
        {
            var data = await GetsAsync(q => q.Where(dut => dut.UserId == userId));

            data.ForEach(dut =>
            {
                dut.UserId = null;
                dut.UpdatedDate = DateTimeOffset.UtcNow;
            });

            await UpdateRangeAsync(data);
        }

        public async Task<HashSet<Guid>> GetEnabledRoles(Guid identifier, IEnumerable<Guid> roleIds)
        {
            return await _dbSet.Where(uut => uut.IsActive && uut.RoleId.HasValue && roleIds.Contains(uut.RoleId.Value) && uut.IsEnabled)
                                   .Where(IdentifierFilter(identifier))
                                   .Select(uut => uut.RoleId!.Value)
                                   .ToHashSetAsync();
        }

        public async Task<Dictionary<Guid, int>> GetRoleCountsAsync(IEnumerable<Guid> identifiers)
        {
            return await _dbSet.Where(uut => uut.IsActive && uut.RoleId.HasValue)
                               .Where(IdentifierContainsFilter(identifiers))
                               .GroupBy(GetIdentifier())
                               .ToDictionaryAsync(c => c.Key!.Value, c => c.Count());
        }
    }
}
