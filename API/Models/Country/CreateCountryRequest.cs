using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Country;

public class CreateCountryRequest
{
    [Mandatory]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Mandatory]
    [MaxLength(45)]
    public string ShortName { get; set; } = string.Empty;
}
