using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_country")]
public class Country : IBaseEntity
{
    [Key]
    [Column("country_id")]
    public Guid Id { get; set; }

    [Column("country_name")]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("country_shortname")]
    [MaxLength(45)]
    public string ShortName { get; set; } = string.Empty;

    [Column("country_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("country_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("country_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("country_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("country_createdby")]
    public string? CreatedBy { get; set; }

    [Column("country_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("country_deletedby")]
    public string? DeletedBy { get; set; }
}
