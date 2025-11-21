using Mapster;
using Sindika.AspNet.app015.Application.DTOs.District;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;

namespace Sindika.AspNet.app015.Application.Services;

public class DistrictService : BaseCrudService<
    DistrictService,
    Context,
    DistrictDTO,
    DistrictPaginationDTO,
    DistrictParam,
    District,
    IDistrictRepository
>, IDistrictService
{
    public DistrictService(
        IConfiguration configuration,
        ILogger<DistrictService> logger,
        IUnitOfWork<Context> unitOfWork,
        IDistrictRepository employeeRepository
    ) : base(configuration, logger, unitOfWork, employeeRepository)
    {
    }

    public async Task<List<DistrictDTO>> GetListAsync(Guid cityId)
    {
        var districts = await _repository.GetsAsync(q => q.Where(c => c.CityId == cityId).OrderBy(c => c.Name));
        return districts.Adapt<List<DistrictDTO>>();
    }
}
