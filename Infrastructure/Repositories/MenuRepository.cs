using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class MenuRepository : BaseRepository<Menu, Context>, IMenuRepository
    {
        public MenuRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<long> GetMenuChildCountAsync(string internalCode)
        {
            var level = internalCode.Length + 4;
            return await _dbSet.Where(m => m.IsActive && m.InternalCode.StartsWith(internalCode) && m.InternalCode.Length == level)
                               .LongCountAsync();
        }
    }
}
