using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_district")]
public class District : IBaseEntity
{
    [Key]
    [Column("district_id")]
    public Guid Id { get; set; }

    [Column("district_provinceid")]
    public Guid ProvinceId { get; set; }
    [ForeignKey(nameof(ProvinceId))]
    public virtual Province? Province { get; set; }

    [Column("district_cityid")]
    public Guid CityId { get; set; }
    [ForeignKey(nameof(CityId))]
    public virtual City? City { get; set; }

    [Column("district_pcode")]
    [MaxLength(100)]
    public string PCode { get; set; } = string.Empty;

    [Column("district_code")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Column("district_name")]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("district_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("district_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("district_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("district_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("district_createdby")]
    public string? CreatedBy { get; set; }

    [Column("district_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("district_deletedby")]
    public string? DeletedBy { get; set; }
}
