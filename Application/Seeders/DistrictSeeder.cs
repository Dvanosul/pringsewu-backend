using Sindika.AspNet.app015.Domain.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class DistrictSeeder
{
    public static List<District> GetDistricts()
    {
        var districts = new List<District>();
        var lines = File.ReadAllLines("Seed/district.txt");

        foreach (var line in lines)
        {
            var columns = line.Split('|');
            var districtId = int.Parse(columns[0]);
            var provinceId = int.Parse(columns[1]);
            var cityId = int.Parse(columns[2]);
            var districtCode = columns[3];
            var districtName = columns[4];
            var districtPcode = columns[5];

            districts.Add(new District
            {
                Id = Guid.Parse($"E0000000-0000-0000-0000-{districtId:D12}"),
                ProvinceId = Guid.Parse($"B0000000-0000-0000-0000-{provinceId:D12}"),
                CityId = Guid.Parse($"D0000000-0000-0000-0000-{cityId:D12}"),
                Code = districtCode,
                PCode = districtPcode,
                Name = districtName,
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            });
        }

        return districts;
    }
}
