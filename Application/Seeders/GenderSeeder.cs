using Sindika.AspNet.app015.Domain.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class GenderSeeder
{
    public static List<Gender> GetGenders()
    {
        return [
            new Gender { Id = Guid.Parse("c86f5bfc-dcda-4dab-9618-aa0bb9eeb163"), Code = "m", Name = "Male"},
            new Gender { Id = Guid.Parse("ff5047f8-c6cf-4c53-a556-deb39c1c2ad2"), Code = "f", Name = "Female"}
        ];
    }
}
