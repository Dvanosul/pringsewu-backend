
namespace Sindika.AspNet.app015.Application.DTOs.Employee
{
    public class UpdateEmployeeParam
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string GenderCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public DateOnly HiredDate { get; set; }
        public IEnumerable<Guid> ChangedRoles { get; set; } = [];
    }
}
