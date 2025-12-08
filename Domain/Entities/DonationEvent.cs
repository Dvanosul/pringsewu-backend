using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    [Table("dbs015_donationevent")]
    public class DonationEvent : IBaseEntity
    {
        [Key]
        [Column("donationevent_id")]
        public Guid Id { get; set; }

        [Column("donationevent_categoryid")]
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        [Column("donationevent_code")]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;
        
        [Column("donationevent_name")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("donationevent_description")]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Column("donationevent_imgurl")]
        [MaxLength(500)]
        public string ImgUrl { get; set; } = string.Empty;

        [Column("donationevent_startdate")]
        public DateTimeOffset StartDate { get; set; }

        [Column("donationevent_enddate")]
        public DateTimeOffset EndDate { get; set; }

        [Column("donationevent_isactive")]
        public bool IsActive { get; set; } = true;

        [Column("donationevent_createddate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("donationevent_updateddate")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Column("donationevent_deleteddate")]
        public DateTimeOffset? DeletedDate { get; set; }

        [Column("donationevent_createdby")]
        public string? CreatedBy { get; set; }

        [Column("donationevent_updatedby")]
        public string? UpdatedBy { get; set; }

        [Column("donation_vent_deletedby")]
        public string? DeletedBy { get; set; }
    }
}
