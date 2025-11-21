using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.SubDistrict;

public class SubDistrictPaginationDTO : PaginationBaseItem
{
    public Guid Id { get; set; }

    public Guid ProvinceId { get; set; }

    public Guid CityId { get; set; }

    public Guid DistrictId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string PCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
