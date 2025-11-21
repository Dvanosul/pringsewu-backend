using Sindika.AspNet.app015.Domain.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class SubDistrictSeeder
{
    public static List<SubDistrict> GetSubDistricts()
    {
        var subDistricts = new List<SubDistrict>();
        var lines = File.ReadAllLines("Seed/subdistricts.txt");

        foreach (var line in lines)
        {
            var columns = line.Split('|');
            var subdistrictId = int.Parse(columns[0]);
            var subdistrictCode = columns[1];
            var subdistrictName = columns[2];
            var districtPcode = columns[3];
            var provinceId = int.Parse(columns[4]);
            var cityId = int.Parse(columns[5]);
            var districtId = int.Parse(columns[6]);

            subDistricts.Add(new SubDistrict
            {
                Id = Guid.Parse($"F0000000-0000-0000-0000-{subdistrictId:D12}"),
                ProvinceId = Guid.Parse($"B0000000-0000-0000-0000-{provinceId:D12}"),
                CityId = Guid.Parse($"D0000000-0000-0000-0000-{cityId:D12}"),
                DistrictId = Guid.Parse($"E0000000-0000-0000-0000-{districtId:D12}"),
                Code = subdistrictCode,
                PCode = districtPcode,
                Name = subdistrictName,
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            });
        }

        return subDistricts;
    }
}
