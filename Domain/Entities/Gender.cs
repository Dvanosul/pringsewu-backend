using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_gender")]
public class Gender : IBaseEntity
{
    [Key]
    [Column("gender_id")]
    public Guid Id { get; set; }

    [Column("gender_name")]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("gender_code")]
    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    [Column("gender_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("gender_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("gender_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("gender_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("gender_createdby")]
    public string? CreatedBy { get; set; }

    [Column("gender_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("gender_deletedby")]
    public string? DeletedBy { get; set; }
}
