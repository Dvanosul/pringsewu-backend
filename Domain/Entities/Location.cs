using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_location")]
public class Location : IBaseEntity
{
    [Key]
    [Column("location_id")]
    public Guid Id { get; set; }

    [Column("location_countryid")]
    public Guid CountryId { get; set; }
    [ForeignKey(nameof(CountryId))]
    public virtual Country? Country { get; set; }

    [Column("location_provinceid")]
    public Guid ProvinceId { get; set; }
    [ForeignKey(nameof(ProvinceId))]
    public virtual Province? Province { get; set; }

    [Column("location_cityid")]
    public Guid CityId { get; set; }
    [ForeignKey(nameof(CityId))]
    public virtual City? City { get; set; }

    [Column("location_districtid")]
    public Guid DistrictId { get; set; }
    [ForeignKey(nameof(DistrictId))]
    public virtual District? District { get; set; }

    [Column("location_subdistrictid")]
    public Guid? SubDistrictId { get; set; }
    public virtual SubDistrict? SubDistrict { get; set; }

    [Column("location_code")]
    [MaxLength(255)]
    public string Code { get; set; } = string.Empty;

    [Column("location_name")]
    [MaxLength(2048)]
    public string Name { get; set; } = string.Empty;

    [Column("location_address")]
    [MaxLength(2048)]
    public string Address { get; set; } = string.Empty;

    [Column("location_latitude", TypeName = "double precision")]
    public double Latitude { get; set; } = 0;

    [Column("location_longitude", TypeName = "double precision")]
    public double Longitude { get; set; } = 0;

    [Column("location_isdefault")]
    public bool IsDefault { get; set; } = false;

    [Column("location_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("location_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("location_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("location_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("location_createdby")]
    public string? CreatedBy { get; set; }

    [Column("location_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("location_deletedby")]
    public string? DeletedBy { get; set; }
}
