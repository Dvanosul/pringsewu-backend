using Sindika.AspNet.app015.Application.DTOs.SubDistrict;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services;

public interface ISubDistrictService : IBaseCrudService<SubDistrictDTO, SubDistrictPaginationDTO, SubDistrictParam>
{
    public Task<List<SubDistrictDTO>> GetListAsync(Guid districtId);
}
