using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.Location
{
    public class UpdateLocationRequest
    {
        [Mandatory]
        [MinLength(1)]
        [MaxLength(255)]
        public string Code { get; set; } = string.Empty;

        [Mandatory]
        [MinLength(3)]
        [MaxLength(2048)]
        public string Name { get; set; } = string.Empty;

        [Mandatory]
        [MinLength(3)]
        [MaxLength(2048)]
        public string Address { get; set; } = string.Empty;

        [Mandatory]
        public Guid CountryId { get; set; }

        [Mandatory]
        public Guid ProvinceId { get; set; }

        [Mandatory]
        public Guid CityId { get; set; }

        [Mandatory]
        public Guid DistrictId { get; set; }

        [Mandatory]
        public Guid SubDistrictId { get; set; }

        [Mandatory]
        public double Latitude { get; set; }

        [Mandatory]
        public double Longitude { get; set; }
    }
}
