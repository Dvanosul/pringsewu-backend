using Sindika.AspNet.app015.Domain.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class CityTypeSeeder
{
    public static List<CityType> GetCityTypes()
    {
        var firstId = 1;
        var secondId = 2;
        return new List<CityType>()
        {
            new()
            {
                Id = Guid.Parse($"C0000000-0000-0000-0000-{firstId:D12}"),
                Code = "KAB",
                Name = "Kabupaten",
                ShortName = "Kab.",
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new()
            {
                Id = Guid.Parse($"C0000000-0000-0000-0000-{secondId:D12}"),
                Code = "KOTA",
                Name = "Kota",
                ShortName = "Kota",
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            },
        };
    }
}
