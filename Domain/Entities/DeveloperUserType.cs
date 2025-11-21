
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    public class DeveloperUserType : UserUserType, IBaseEntity
    {
        [Column("user_usertype_role_developerid")]
        public Guid? DeveloperId { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer? Developer { get; set; }
    }
}
