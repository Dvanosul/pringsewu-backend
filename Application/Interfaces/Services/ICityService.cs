using Sindika.AspNet.app015.Application.DTOs.City;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services;

public interface ICityService : IBaseCrudService<CityDTO, CityPaginationDTO, CityParam>
{
    Task<List<CityDTO>> GetListAsync(Guid provinceId);
}
