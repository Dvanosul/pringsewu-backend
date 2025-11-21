using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    [Table("dbs015_employee")]
    public class Employee : IBaseEntity
    {
        [Key]
        [Column("employee_id")]
        public Guid Id { get; set; }

        [Column("employee_name")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("employee_code")]
        [MaxLength(255)]
        public string Code { get; set; } = string.Empty;

        [Column("employee_phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("employee_gendercode")]
        [MaxLength(255)]
        public string GenderCode { get; set; } = null!;

        [Column("employee_address")]
        [MaxLength(255)]
        public string Address { get; set; } = null!;

        [Column("employee_birthdate")]
        public DateOnly BirthDate { get; set; }

        [Column("employee_hireddate")]
        public DateOnly HiredDate { get; set; }

        [Column("employee_isactive")]
        public bool IsActive { get; set; } = true;

        [Column("employee_createddate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("employee_updateddate")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Column("employee_deleteddate")]
        public DateTimeOffset? DeletedDate { get; set; }

        [Column("employee_createdby")]
        public string? CreatedBy { get; set; }

        [Column("employee_updatedby")]
        public string? UpdatedBy { get; set; }

        [Column("employee_deletedby")]
        public string? DeletedBy { get; set; }
    }
}
