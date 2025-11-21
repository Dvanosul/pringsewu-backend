using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class UserTypeRepository : BaseRepository<UserType, Context>, IUserTypeRepository
    {
        public UserTypeRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<UserType?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(ut => ut.Code == code);
        }
    }
}
