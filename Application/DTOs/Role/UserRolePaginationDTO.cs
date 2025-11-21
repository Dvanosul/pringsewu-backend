using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Role
{
    public class UserRolePaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public IEnumerable<string> PageAccess { get; set; } = [];
    }
}
