namespace Sindika.AspNet.app015.Application.DTOs.DonationEvent
{
    public class CreateDonationEventParam
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateDonationEventParam
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateDonationEventStatusParam
    {
        public bool IsActive { get; set; }
    }
}
