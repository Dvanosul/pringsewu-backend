namespace Sindika.AspNet.app015.Application.DTOs.Province;

public class ProvinceParam
{
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;
}
