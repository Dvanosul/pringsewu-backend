using Sindika.AspNet.app015.Application.DTOs.City;
using Sindika.AspNet.app015.Application.DTOs.Country;
using Sindika.AspNet.app015.Application.DTOs.District;
using Sindika.AspNet.app015.Application.DTOs.Province;
using Sindika.AspNet.Response;
namespace Sindika.AspNet.app015.Application.DTOs.Location;

public class LocationParam
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid CountryId { get; set; }
    public Guid ProvinceId { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrictId { get; set; }
    public Guid SubDistrictId { get; set; }
}
