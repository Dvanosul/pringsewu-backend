using Mapster;
using Sindika.AspNet.app015.Application.DTOs.Province;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;

namespace Sindika.AspNet.app015.Application.Services;

public class ProvinceService : BaseCrudService<
    ProvinceService,
    Context,
    ProvinceDTO,
    ProvincePaginationDTO,
    ProvinceParam,
    Province,
    IProvinceRepository
>, IProvinceService
{
    public ProvinceService(
        IConfiguration configuration,
        ILogger<ProvinceService> logger,
        IUnitOfWork<Context> unitOfWork,
        IProvinceRepository employeeRepository
    ) : base(configuration, logger, unitOfWork, employeeRepository)
    {
    }

    public async Task<List<ProvinceDTO>> GetListAsync(Guid countryId)
    {
        var provinces = await _repository.GetsAsync(q => q.Where(c => c.CountryId == countryId).OrderBy(c => c.Name));
        return provinces.Adapt<List<ProvinceDTO>>();
    }
}
