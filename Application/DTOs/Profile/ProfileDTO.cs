namespace Sindika.AspNet.app015.Application.DTOs.Profile
{
    public class ProfileDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
