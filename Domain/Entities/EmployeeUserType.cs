
using System.ComponentModel.DataAnnotations.Schema;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Domain.Entities
{
    public class EmployeeUserType : UserUserType, IBaseEntity
    {
        [Column("user_usertype_role_employeeid")]
        public Guid? EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
    }
}
