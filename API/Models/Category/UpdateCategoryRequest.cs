using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.Category
{
    public class UpdateCategoryRequest
    {
        [Mandatory]
        [MinLength(3)]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [Mandatory]
        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
    }
}
