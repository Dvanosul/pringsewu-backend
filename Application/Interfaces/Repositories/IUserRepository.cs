using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> IsSuperAsync(Guid id);
        Task<bool> IsEmailUniqueAsync(string email, Guid? id = null);
    }
}
