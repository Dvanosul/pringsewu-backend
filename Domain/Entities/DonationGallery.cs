using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    [Table("dbs015_donationgallery")]
    public class DonationGallery : IBaseEntity
    {
        [Key]
        [Column("donationgallery_id")]
        public Guid Id { get; set; }

        [Column("donationgallery_eventid")]
        public Guid DonationEventId { get; set; }
        [ForeignKey(nameof(DonationEventId))]
        public virtual DonationEvent? DonationEvent { get; set; }

        [Column("donationgallery_imgurl")]
        [MaxLength(500)]
        public string ImgUrl { get; set; } = string.Empty;

        [Column("donationgallery_description")]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Column("donationgallery_isactive")]
        public bool IsActive { get; set; } = true;

        [Column("donationgallery_createddate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("donationgallery_updateddate")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Column("donationgallery_deleteddate")]
        public DateTimeOffset? DeletedDate { get; set; }

        [Column("donationgallery_createdby")]
        public string? CreatedBy { get; set; }

        [Column("donationgallery_updatedby")]
        public string? UpdatedBy { get; set; }

        [Column("donationgallery_deletedby")]
        public string? DeletedBy { get; set; }

    }
}
