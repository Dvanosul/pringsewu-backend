using Sindika.AspNet.app015.Domain.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class CitySeeder
{
    public static List<City> GetCities()
    {
        var cities = new List<City>();
        var lines = File.ReadAllLines("Seed/city.txt");

        foreach (var line in lines)
        {
            var columns = line.Split('|');
            var cityId = int.Parse(columns[0]);
            var provinceId = int.Parse(columns[1]);
            var cityTypeId = int.Parse(columns[2]);
            var countryId = int.Parse(columns[3]);
            var cityCode = columns[4];
            var cityName = columns[5];

            cities.Add(new City
            {
                Id = Guid.Parse($"D0000000-0000-0000-0000-{cityId:D12}"),
                ProvinceId = Guid.Parse($"B0000000-0000-0000-0000-{provinceId:D12}"),
                CityTypeId = Guid.Parse($"C0000000-0000-0000-0000-{cityTypeId:D12}"),
                CountryId = Guid.Parse($"A0000000-0000-0000-0000-{countryId:D12}"),
                Code = cityCode,
                Name = cityName,
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            });
        }

        return cities;
    }
}
