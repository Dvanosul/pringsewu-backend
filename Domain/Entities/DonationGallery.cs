using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    [Table("dbs015_donation_gallery")]
    public class DonationGallery : IBaseEntity
    {
        [Key]
        [Column("donation_gallery_id")]
        public Guid Id { get; set; }

        [Column("donation_gallery_eventid")]
        public Guid EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual DonationEvent? Event { get; set; }

        [Column("donation_gallery_imgurl")]
        [MaxLength(500)]
        public string ImgUrl { get; set; } = string.Empty;

        [Column("donation_gallery_description")]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Column("donation_gallery_isactive")]
        public bool IsActive { get; set; } = true;

        [Column("donation_gallery_createddate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("donation_gallery_updateddate")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Column("donation_gallery_deleteddate")]
        public DateTimeOffset? DeletedDate { get; set; }

        [Column("donation_gallery_createdby")]
        public string? CreatedBy { get; set; }

        [Column("donation_gallery_updatedby")]
        public string? UpdatedBy { get; set; }

        [Column("donation_gallery_deletedby")]
        public string? DeletedBy { get; set; }

    }
}
