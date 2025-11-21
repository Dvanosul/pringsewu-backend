
namespace Sindika.AspNet.app015.Application.DTOs.User
{
    public class CreateUserParam
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid? LanguageId { get; set; }
        public Guid? ZoneId { get; set; }
        public string? UserTypeCode { get; set; }
        public Guid? DeveloperId { get; set; }
        public Guid? EmployeeId { get; set; }
        public bool IsSuper { get; set; }
        public string? Image { get; set; }
        public string CreateBy { get; set; } = string.Empty;
    }
}
