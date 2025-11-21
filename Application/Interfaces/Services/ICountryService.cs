using Sindika.AspNet.app015.Application.DTOs.Country;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services;

public interface ICountryService : IBaseCrudService<CountryDTO, CountryPaginationDTO, CountryParam>
{
    public Task<List<CountryDTO>> GetListAsync();
}
