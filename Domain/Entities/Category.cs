using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities;

[Table("dbs015_category")]
public class Category : IBaseEntity
{
    [Key]
    [Column("category_id")]
    public Guid Id { get; set; }

    [Column("category_code")]
    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    [Column("category_name")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Column("category_isactive")]
    public bool IsActive { get; set; } = true;

    [Column("category_createddate")]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    [Column("category_updateddate")]
    public DateTimeOffset? UpdatedDate { get; set; }

    [Column("category_deleteddate")]
    public DateTimeOffset? DeletedDate { get; set; }

    [Column("category_createdby")]
    public string? CreatedBy { get; set; }

    [Column("category_updatedby")]
    public string? UpdatedBy { get; set; }

    [Column("category_deletedby")]
    public string? DeletedBy { get; set; }
}
