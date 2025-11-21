using Sindika.AspNet.app015.Application.DTOs.Province;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services;

public interface IProvinceService : IBaseCrudService<ProvinceDTO, ProvincePaginationDTO, ProvinceParam>
{
    Task<List<ProvinceDTO>> GetListAsync(Guid countryId);
}
