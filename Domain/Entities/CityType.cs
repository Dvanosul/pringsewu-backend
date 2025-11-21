using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_citytype")]
public class CityType : IBaseEntity
{
    [Key]
    [Column("citytype_id")]
    public Guid Id { get; set; }

    [Column("citytype_code")]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [Column("citytype_name")]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("citytype_shortname")]
    [MaxLength(45)]
    public string ShortName { get; set; } = string.Empty;

    [Column("citytype_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("citytype_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("citytype_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("citytype_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("citytype_createdby")]
    public string? CreatedBy { get; set; }

    [Column("citytype_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("citytype_deletedby")]
    public string? DeletedBy { get; set; }
}
