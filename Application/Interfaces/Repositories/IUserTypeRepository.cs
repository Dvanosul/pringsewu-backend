using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IUserTypeRepository : IBaseRepository<UserType>
    {
        Task<UserType?> GetByCodeAsync(string code);
    }
}
