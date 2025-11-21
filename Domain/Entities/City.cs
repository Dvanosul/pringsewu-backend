using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_city")]
public class City : IBaseEntity
{
    [Key]
    [Column("city_id")]
    public Guid Id { get; set; }

    [Column("city_provinceid")]
    public Guid ProvinceId { get; set; }
    [ForeignKey(nameof(ProvinceId))]
    public virtual Province? Province { get; set; }

    [Column("city_citytypeid")]
    public Guid CityTypeId { get; set; }
    [ForeignKey(nameof(CityTypeId))]
    public virtual CityType? CityType { get; set; }

    [Column("city_countryid")]
    public Guid CountryId { get; set; }
    [ForeignKey(nameof(CountryId))]
    public virtual Country? Country { get; set; }

    [Column("city_code")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Column("city_name")]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("city_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("city_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("city_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("city_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("city_createdby")]
    public string? CreatedBy { get; set; }

    [Column("city_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("city_deletedby")]
    public string? DeletedBy { get; set; }
}
