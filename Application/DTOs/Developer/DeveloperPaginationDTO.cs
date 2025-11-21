
using System.Text.Json.Serialization;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Developer
{
    public class DeveloperPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Roles { get; set; }
    }
}
