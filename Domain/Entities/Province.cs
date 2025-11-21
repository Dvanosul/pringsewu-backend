using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_province")]
public class Province : IBaseEntity
{
    [Key]
    [Column("province_id")]
    public Guid Id { get; set; }

    [Column("province_countryid")]
    public Guid CountryId { get; set; }
    [ForeignKey(nameof(CountryId))]
    public virtual Country? Country { get; set; }

    [Column("province_code")]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [Column("province_name")]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("province_shortname")]
    [MaxLength(45)]
    public string ShortName { get; set; } = string.Empty;

    [Column("province_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("province_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("province_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("province_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("province_createdby")]
    public string? CreatedBy { get; set; }

    [Column("province_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("province_deletedby")]
    public string? DeletedBy { get; set; }
}
