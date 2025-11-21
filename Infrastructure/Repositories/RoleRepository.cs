using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role, Context>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<Role?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.IsActive && c.Code == code);
        }

        public async Task<List<Role>> GetsByCodeAsync(List<string>? codes = null)
        {
            if (codes == null) return await _dbSet.Include(c => c.UserType).Where(c => c.IsActive).ToListAsync();
            var roles = await _dbSet.Include(c => c.UserType).Where(c => c.IsActive && codes.Contains(c.Code)).ToListAsync();
            return roles;
        }

        public async Task<bool> IsAllExistAsync(IEnumerable<Guid> ids)
        {
            var count = await _dbSet.Where(q => q.IsActive && ids.Contains(q.Id)).LongCountAsync();

            return count == ids.LongCount();
        }

        public async Task<bool> IsExistAsync(Guid id, string discriminator)
        {
            return await _dbSet.AnyAsync(c => c.IsActive && c.Id == id && c.UserType!.Code == discriminator);
        }
    }
}
