using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories;

public interface IBaseUserTypeRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    Task<ItemCountResponse<TEntity>> GetAvailablePaginationAsync(PaginationQuery paginationQuery, Guid? userId = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
}
