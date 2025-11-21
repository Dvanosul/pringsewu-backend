using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class BaseUserTypeRepository<TEntity, TUEntity, TContext> : BaseRepository<TEntity, TContext>, IBaseUserTypeRepository<TEntity> where TEntity : class, IBaseEntity where TUEntity : UserUserType where TContext : DbContext
    {
        private readonly IBaseUserUserTypeRepository<TUEntity, TEntity> _userUserTypeRepository;

        public BaseUserTypeRepository(IUnitOfWork<TContext> unitOfWork, IBaseUserUserTypeRepository<TUEntity, TEntity> userUserTypeRepository) : base(unitOfWork)
        {
            _userUserTypeRepository = userUserTypeRepository;
        }

        public async Task<ItemCountResponse<TEntity>> GetAvailablePaginationAsync(PaginationQuery paginationQuery, Guid? userId = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            var source = _dbSet.AsQueryable();
            source = source.Where((c) => c.IsActive).Where(_userUserTypeRepository.IsAvailableFilter(userId));

            if (include != null)
            {
                source = include(source);
            }

            if (paginationQuery.Filters.Any())
            {
                source = source.Where(BuildDynamicFilter<TEntity>(paginationQuery));
            }

            paginationQuery.AddDefaultSort("createddate", "desc");
            return await BuildPaginationQuery(paginationQuery, source);
        }
    }
}
