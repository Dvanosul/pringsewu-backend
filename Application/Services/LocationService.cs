using Mapster;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.Location;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;
using Sindika.AspNet.Exceptions.NotFound;

namespace Sindika.AspNet.app015.Application.Services;

public class LocationService : BaseCrudService<
    LocationService,
    Context,
    LocationDTO,
    LocationPaginationDTO,
    LocationParam,
    Location,
    ILocationRepository
>, ILocationService
{
    public LocationService(
        IConfiguration configuration,
        ILogger<LocationService> logger,
        IUnitOfWork<Context> unitOfWork,
        ILocationRepository locationRepository
    ) : base(configuration, logger, unitOfWork, locationRepository)
    {
    }

    public new async Task<LocationDTO> GetAsync(Guid id)
    {
        try
        {
            StartOperation("GET");
            var location = await _repository.GetAsync(id, q => q
                .Include(x => x.Country)
                .Include(x => x.Province)
                .Include(x => x.City)
                .Include(x => x.District)
                .Include(x => x.SubDistrict)
            ) ?? throw new NotFoundException($"Location with ID {id} not found");
            var result = location.Adapt<LocationDTO>();
            AppendRecords(location.Id.ToString());
            return result;
        }
        finally
        {
            EndOperation();
        }
    }

    public new async Task<PaginationResponse<LocationPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
    {
        try
        {
            StartOperation("GET");
            ItemCountResponse<Location> itemCountResponse = await _repository.GetPaginationAsync(
                paginationQuery,
                q => q
                    .Include(x => x.Country)
                    .Include(x => x.Province)
                    .Include(x => x.City)
                    .Include(x => x.District)
                    .Include(x => x.SubDistrict)
            );
            List<LocationPaginationDTO> items = itemCountResponse.Items.Adapt<List<LocationPaginationDTO>>();
            PaginationResponse<LocationPaginationDTO> result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
            AppendRecords(null, itemCountResponse.Items.Select((Location c) => c.Id.ToString()).ToList());
            return result;
        }
        finally
        {
            EndOperation();
        }
    }
}
