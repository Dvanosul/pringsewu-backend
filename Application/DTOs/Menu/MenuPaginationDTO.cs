using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Menu
{
    public class MenuPaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
