using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class ZoneRepository : BaseRepository<Zone, Context>, IZoneRepository
    {
        public ZoneRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }
    }
}
