namespace Sindika.AspNet.app015.Application.DTOs.City;

public class CityParam
{
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }

    public Guid CityTypeId { get; set; }

    public Guid ProvinceId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
