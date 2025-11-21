using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    [Table("dbs000_developer")]
    public class Developer : IBaseEntity
    {
        [Key]
        [Column("developer_id")]
        public Guid Id { get; set; }

        [Column("developer_name")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("developer_code")]
        [MaxLength(255)]
        public string Code { get; set; } = string.Empty;

        [Column("developer_phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("developer_isactive")]
        public bool IsActive { get; set; } = true;

        [Column("developer_createddate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("developer_updateddate")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Column("developer_deleteddate")]
        public DateTimeOffset? DeletedDate { get; set; }

        [Column("developer_createdby")]
        public string? CreatedBy { get; set; }

        [Column("developer_updatedby")]
        public string? UpdatedBy { get; set; }

        [Column("developer_deletedby")]
        public string? DeletedBy { get; set; }
    }
}
