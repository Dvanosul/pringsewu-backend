using Sindika.AspNet.app015.Application.DTOs.Zone;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IZoneService : IBaseCrudService<ZoneDTO, ZonePaginationDTO, CreateZoneParam>
    {

    }
}
