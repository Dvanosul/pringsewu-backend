using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<long> GetMenuChildCountAsync(string internalCode);
    }
}
