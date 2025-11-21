namespace Sindika.AspNet.app015.Application.DTOs.Event
{
    public class EventDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = true;
    }
}
