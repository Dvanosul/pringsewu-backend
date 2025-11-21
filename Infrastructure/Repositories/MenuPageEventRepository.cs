using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class MenuPageEventRepository : BaseRepository<MenuPageEvent, Context>, IMenuPageEventRepository
    {
        public MenuPageEventRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<MenuPageEvent?> GetByMenuIdAsync(Guid menuId, Func<IQueryable<MenuPageEvent>, IQueryable<MenuPageEvent>>? include = null)
        {
            var query = _dbSet.Where(m => m.IsActive && m.MenuId == menuId);

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
