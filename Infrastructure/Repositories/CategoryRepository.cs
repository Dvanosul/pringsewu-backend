using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Builders;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category, Context>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<bool> IsExistByCodeAsync(string code)
        {
            return await _dbSet.TagWithCallSiteGlobal().AnyAsync((c) => c.IsActive && c.Code == code);
        }
    }
}
