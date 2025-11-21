using Mapster;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.City;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Services;

public class CityService : BaseCrudService<
    CityService,
    Context,
    CityDTO,
    CityPaginationDTO,
    CityParam,
    City,
    ICityRepository
>, ICityService
{
    public CityService(
        IConfiguration configuration,
        ILogger<CityService> logger,
        IUnitOfWork<Context> unitOfWork,
        ICityRepository employeeRepository
    ) : base(configuration, logger, unitOfWork, employeeRepository)
    {
    }

    public new async Task<PaginationResponse<CityPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
    {
        try
        {
            StartOperation("GET");
            ItemCountResponse<City> itemCountResponse = await _repository.GetPaginationAsync(
                paginationQuery,
                q => q
                    .Include(x => x.CityType)
                    .Include(x => x.Country)
                    .Include(x => x.Province)
            );
            List<CityPaginationDTO> items = itemCountResponse.Items.Adapt<List<CityPaginationDTO>>();
            PaginationResponse<CityPaginationDTO> result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
            AppendRecords(null, itemCountResponse.Items.Select((City c) => c.Id.ToString()).ToList());
            return result;
        }
        finally
        {
            EndOperation();
        }
    }

    public async Task<List<CityDTO>> GetListAsync(Guid provinceId)
    {
        var cities = await _repository.GetsAsync(q => q
            .Where(c => c.ProvinceId == provinceId)
            .OrderBy(c => c.Name)
            .Include(c => c.CityType));
        return cities.Adapt<List<CityDTO>>();
    }
}
