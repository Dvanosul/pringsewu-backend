
using Sindika.AspNet.app015.Application.DTOs.Role;

namespace Sindika.AspNet.app015.Application.DTOs.User
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Image { get; set; }
        public IEnumerable<RoleDTO> Roles { get; set; } = [];
    }
}
