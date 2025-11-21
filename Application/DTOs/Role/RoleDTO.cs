using System.Text.Json.Serialization;
using Sindika.AspNet.app015.Application.DTOs.UserType;

namespace Sindika.AspNet.app015.Application.DTOs.Role
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserTypeDTO? UserType { get; set; }
    }
}
