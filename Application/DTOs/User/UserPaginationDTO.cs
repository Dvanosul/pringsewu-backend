
using Sindika.AspNet.app015.Application.DTOs.Language;
using Sindika.AspNet.app015.Application.DTOs.Zone;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.User
{
    public class UserPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> UserTypes { get; set; } = [];
        public LanguageDTO? Language { get; set; }
        public bool IsSuper { get; set; }
        public ZoneDTO? Zone { get; set; }
    }
}
