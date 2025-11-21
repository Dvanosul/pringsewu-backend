using Sindika.AspNet.app015.Application.DTOs.Country;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.City;

public class CityPaginationDTO : PaginationBaseItem
{
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }
    public CountryDTO? Country { get; set; }

    public Guid CityTypeId { get; set; }
    public CityTypeDTO? CityType { get; set; }

    public Guid CityId { get; set; }
    public CityDTO? City { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
