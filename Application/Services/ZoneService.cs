using Sindika.AspNet.app015.Application.DTOs.Zone;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Entities;

namespace Sindika.AspNet.app015.Application.Services
{
    public class ZoneService : BaseCrudService<
        ZoneService,
        Context,
        ZoneDTO,
        ZonePaginationDTO,
        CreateZoneParam,
        Zone,
        IZoneRepository>, IZoneService
    {
        public ZoneService(
            IConfiguration configuration,
            ILogger<ZoneService> logger,
            IUnitOfWork<Context>
            unitOfWork, IZoneRepository repository
            ) : base(configuration, logger, unitOfWork, repository)
        {
        }

    }
}
