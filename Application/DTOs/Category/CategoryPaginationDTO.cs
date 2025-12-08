using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Category
{
    public class CategoryPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
