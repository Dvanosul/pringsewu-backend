using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Province;

public class CreateProvinceRequest
{
    [Mandatory]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Mandatory]
    [JsonPropertyName("country_id")]
    public Guid CountryId { get; set; }

    [Mandatory]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [Mandatory]
    [JsonPropertyName("short_name")]
    [MaxLength(45)]
    public string ShortName { get; set; } = string.Empty;

}
