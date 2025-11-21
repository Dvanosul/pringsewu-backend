
using System.Text.Json.Serialization;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.DTOs.Employee
{
    public class EmployeePaginationDTO : PaginationBaseItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public DateOnly HiredDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Roles { get; set; }
    }
}
