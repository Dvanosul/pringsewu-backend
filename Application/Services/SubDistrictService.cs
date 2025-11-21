using Mapster;
using Sindika.AspNet.app015.Application.DTOs.SubDistrict;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;

namespace Sindika.AspNet.app015.Application.Services;

public class SubDistrictService : BaseCrudService<
    SubDistrictService,
    Context,
    SubDistrictDTO,
    SubDistrictPaginationDTO,
    SubDistrictParam,
    SubDistrict,
    ISubDistrictRepository
>, ISubDistrictService
{
    public SubDistrictService(
        IConfiguration configuration,
        ILogger<SubDistrictService> logger,
        IUnitOfWork<Context> unitOfWork,
        ISubDistrictRepository employeeRepository
    ) : base(configuration, logger, unitOfWork, employeeRepository)
    {
    }

    public async Task<List<SubDistrictDTO>> GetListAsync(Guid districtId)
    {
        var subDistricts =
            await _repository.GetsAsync(q => q.Where(c => c.DistrictId == districtId).OrderBy(c => c.Name));
        return subDistricts.Adapt<List<SubDistrictDTO>>();
    }
}
