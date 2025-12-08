namespace Sindika.AspNet.app015.Application.DTOs.DonationEvent
{
    public class DonationEventParam
    {
        public Guid CategoryId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
