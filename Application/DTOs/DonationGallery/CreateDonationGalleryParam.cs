namespace Sindika.AspNet.app015.Application.DTOs.DonationGallery
{
    public class CreateDonationGalleryParam
    {
        public Guid EventId { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateDonationGalleryParam
    {
        public Guid EventId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
