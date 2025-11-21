using Sindika.AspNet.app015.Application.DTOs.Developer;
using Sindika.AspNet.app015.Application.DTOs.Employee;
using Sindika.AspNet.app015.Application.DTOs.Language;
using Sindika.AspNet.app015.Application.DTOs.Zone;

namespace Sindika.AspNet.app015.Application.DTOs.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public LanguageDTO? Language { get; set; }
        public ZoneDTO? Zone { get; set; }
        public DeveloperDTO? Developer { get; set; }
        public EmployeeDTO? Employee { get; set; }
        public bool IsSuper { get; set; }
        public string? Image { get; set; }
    }
}
