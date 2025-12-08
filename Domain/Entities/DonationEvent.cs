using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    [Table("dbs015_donation_event")]
    public class DonationEvent : IBaseEntity
    {
        [Key]
        [Column("donation_event_id")]
        public Guid Id { get; set; }

        [Column("donation_event_categoryid")]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Column("donation_event_code")]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [Column("donation_event_name")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("donation_event_description")]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Column("donation_event_imgurl")]
        [MaxLength(500)]
        public string ImgUrl { get; set; } = string.Empty;

        [Column("donation_event_startdate")]
        public DateTimeOffset StartDate { get; set; }

        [Column("donation_event_enddate")]
        public DateTimeOffset EndDate { get; set; }

        [Column("donation_event_isactive")]
        public bool IsActive { get; set; } = true;

        [Column("donation_event_createddate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("donation_event_updateddate")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Column("donation_event_deleteddate")]
        public DateTimeOffset? DeletedDate { get; set; }

        [Column("donation_event_createdby")]
        public string? CreatedBy { get; set; }

        [Column("donation_event_updatedby")]
        public string? UpdatedBy { get; set; }

        [Column("donation_event_deletedby")]
        public string? DeletedBy { get; set; }
    }
}
