using Sindika.AspNet.app015.Application.DTOs.Country;
using Sindika.AspNet.app015.Application.DTOs.Province;

namespace Sindika.AspNet.app015.Application.DTOs.City;

public class CityDTO
{
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }
    public CountryDTO? Country { get; set; }

    public Guid CityTypeId { get; set; }

    public CityTypeDTO? CityType { get; set; }

    public Guid ProvinceId { get; set; }
    public ProvinceDTO? Province { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

public class CityTypeDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
