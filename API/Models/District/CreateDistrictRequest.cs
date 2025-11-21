using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.District;

public class CreateDistrictRequest
{
    [Mandatory]
    [JsonPropertyName("province_id")]
    public Guid ProvinceId { get; set; }

    [Mandatory]
    [JsonPropertyName("city_id")]
    public Guid CityId { get; set; }

    [Mandatory]
    public string Code { get; set; } = string.Empty;

    [Mandatory]
    [JsonPropertyName("p_code")]
    public string PCode { get; set; } = string.Empty;

    [Mandatory]
    public string Name { get; set; } = string.Empty;
}
