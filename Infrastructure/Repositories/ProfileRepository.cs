using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class ProfileRepository : BaseRepository<Profile, Context>, IProfileRepository
    {
        public ProfileRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<bool> IsAllExistAsync(IEnumerable<Guid> ids)
        {
            var count = await _dbSet.Where(q => q.IsActive && ids.Contains(q.Id)).LongCountAsync();

            return count == ids.LongCount();
        }
    }
}
