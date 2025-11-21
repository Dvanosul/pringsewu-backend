using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Country;

public class CountryPaginationDTO : PaginationBaseItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;

}
