using Sindika.AspNet.app015.Domain.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class CountrySeeder
{
    public static List<Country> GetCountries()
    {
        var id = 1;
        return new List<Country>()
        {
            new()
            {
                Id = Guid.Parse($"A0000000-0000-0000-0000-{id:D12}"),
                Name = "Indonesia",
                ShortName = "ID",
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            }
        };
    }
}
