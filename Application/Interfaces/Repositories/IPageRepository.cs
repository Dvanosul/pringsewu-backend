using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IPageRepository : IBaseRepository<Page>
    {
        Task<bool> IsAllExistAsync(IEnumerable<Guid> ids);
    }
}
