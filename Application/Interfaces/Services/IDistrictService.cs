using Sindika.AspNet.app015.Application.DTOs.District;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services;

public interface IDistrictService : IBaseCrudService<DistrictDTO, DistrictPaginationDTO, DistrictParam>
{
    Task<List<DistrictDTO>> GetListAsync(Guid cityId);
}
