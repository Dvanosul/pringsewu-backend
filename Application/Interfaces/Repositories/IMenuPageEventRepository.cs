using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IMenuPageEventRepository : IBaseRepository<MenuPageEvent>
    {
        Task<MenuPageEvent?> GetByMenuIdAsync(Guid menuId, Func<IQueryable<MenuPageEvent>, IQueryable<MenuPageEvent>>? include = null);
    }
}
