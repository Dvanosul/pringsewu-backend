using Sindika.AspNet.Common.Entities;

namespace Sindika.AspNet.app015.Application.Seeders;

public static class UserTypeSeeder
{
    public static List<UserType> GetUserTypes()
    {
        return new List<UserType>
        {
            new UserType 
            { 
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                Code = "EmployeeUserType", 
                Name = "Employee User Type",
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new UserType 
            { 
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), 
                Code = "DeveloperUserType", 
                Name = "Developer User Type",
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            }
        };
    }
}
