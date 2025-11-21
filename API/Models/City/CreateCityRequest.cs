using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.City;

public class CreateCityRequest
{
    [Mandatory]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Mandatory]
    [JsonPropertyName("country_id")]
    public Guid CountryId { get; set; }

    [Mandatory]
    [JsonPropertyName("city_type_id")]
    public Guid CityTypeId { get; set; }

    [Mandatory]
    [JsonPropertyName("province_id")]
    public Guid ProvinceId { get; set; }

    [Mandatory]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;
}
