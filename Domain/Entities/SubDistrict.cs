using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_subdistrict")]
public class SubDistrict : IBaseEntity
{
    [Key]
    [Column("subdistrict_id")]
    public Guid Id { get; set; }

    [Column("subdistrict_provinceid")]
    public Guid ProvinceId { get; set; }
    [ForeignKey(nameof(ProvinceId))]
    public virtual Province? Province { get; set; }

    [Column("subdistrict_cityid")]
    public Guid CityId { get; set; }
    [ForeignKey(nameof(CityId))]
    public virtual City? City { get; set; }

    [Column("subdistrict_districtid")]
    public Guid DistrictId { get; set; }
    [ForeignKey(nameof(DistrictId))]
    public virtual District? District { get; set; }

    [Column("subdistrict_pcode")]
    [MaxLength(100)]
    public string PCode { get; set; } = string.Empty;

    [Column("subdistrict_code")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Column("subdistrict_name")]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("subdistrict_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("subdistrict_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("subdistrict_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("subdistrict_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("subdistrict_createdby")]
    public string? CreatedBy { get; set; }

    [Column("subdistrict_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("subdistrict_deletedby")]
    public string? DeletedBy { get; set; }
}
