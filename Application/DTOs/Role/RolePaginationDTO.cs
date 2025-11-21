using System.Text.Json.Serialization;
using Sindika.AspNet.app015.Application.DTOs.UserType;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Role
{
    public class RolePaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserTypeDTO? UserType { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsEnabled { get; set; }
    }
}
