using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;
using Sindika.AspNet.Validation.Attributes.String;

namespace Sindika.AspNet.app015.API.Models.DonationEvent
{
    public class UpdateDonationEventRequest
    {
        [Mandatory]
        public Guid CategoryId { get; set; }
        
        [Mandatory]
        [MinLength(3)]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [Mandatory]
        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public IFormFile? ImgUrl { get; set; }

        [Mandatory]
        public DateTimeOffset StartDate { get; set; }

        [Mandatory]
        public DateTimeOffset EndDate { get; set; }
    }
}
