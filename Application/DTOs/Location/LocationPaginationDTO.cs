using Sindika.AspNet.app015.Application.DTOs.City;
using Sindika.AspNet.app015.Application.DTOs.Country;
using Sindika.AspNet.app015.Application.DTOs.District;
using Sindika.AspNet.app015.Application.DTOs.Province;
using Sindika.AspNet.app015.Application.DTOs.SubDistrict;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Location;

public class LocationPaginationDTO : PaginationBaseItem
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsDefault { get; set; } = false;
    public Guid CountryId { get; set; }
    public CountryDTO? Country { get; set; }
    public Guid ProvinceId { get; set; }
    public ProvinceDTO? Province { get; set; }
    public Guid CityId { get; set; }
    public CityDTO? City { get; set; }
    public Guid DistrictId { get; set; }
    public DistrictDTO? District { get; set; }
    public Guid SubDistrictId { get; set; }
    public SubDistrictDTO? SubDistrict { get; set; }
}
