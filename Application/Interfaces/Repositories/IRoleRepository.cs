using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<List<Role>> GetsByCodeAsync(List<string>? codes = null);
        Task<Role?> GetByCodeAsync(string code);
        Task<bool> IsAllExistAsync(IEnumerable<Guid> ids);
        Task<bool> IsExistAsync(Guid id, string discriminator);
    }
}
