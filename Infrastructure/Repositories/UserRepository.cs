using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User, Context>, IUserRepository
    {
        public UserRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<bool> IsSuperAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.IsActive && u.Id == id && u.IsSuper) != null;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, Guid? id = null)
        {
            var query = _dbSet.Where(u => u.IsActive && u.Email == email);
            if (id != null) query = query.Where(u => u.Id != id);

            return !await query.AnyAsync();
        }
    }
}
