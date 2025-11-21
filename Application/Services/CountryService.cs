using Mapster;
using Sindika.AspNet.app015.Application.DTOs.Country;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;

namespace Sindika.AspNet.app015.Application.Services;

public class CountryService : BaseCrudService<
    CountryService,
    Context,
    CountryDTO,
    CountryPaginationDTO,
    CountryParam,
    Country,
    ICountryRepository
>, ICountryService
{
    public CountryService(
        IConfiguration configuration,
        ILogger<CountryService> logger,
        IUnitOfWork<Context> unitOfWork,
        ICountryRepository employeeRepository
    ) : base(configuration, logger, unitOfWork, employeeRepository) { }

    public async Task<List<CountryDTO>> GetListAsync()
    {
        var countries = await _repository.GetsAsync();
        return countries.Adapt<List<CountryDTO>>();
    }
}
