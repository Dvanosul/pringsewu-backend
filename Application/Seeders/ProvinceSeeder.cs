using Sindika.AspNet.app015.Domain.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class ProvinceSeeder
{
    public static List<Province> GetProvinces()
    {
        var provinces = new List<Province>();
        var lines = File.ReadAllLines("Seed/province.txt");

        foreach (var line in lines)
        {
            var columns = line.Split('|');
            var provinceId = int.Parse(columns[0]);
            var countryId = int.Parse(columns[1]);
            var provinceCode = columns[2];
            var provinceName = columns[3];
            var provinceShortName = columns[4];

            provinces.Add(new Province
            {
                Id = Guid.Parse($"B0000000-0000-0000-0000-{provinceId:D12}"),
                CountryId = Guid.Parse($"A0000000-0000-0000-0000-{countryId:D12}"),
                Code = provinceCode,
                Name = provinceName,
                ShortName = provinceShortName,
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            });
        }

        return provinces;
    }
}
