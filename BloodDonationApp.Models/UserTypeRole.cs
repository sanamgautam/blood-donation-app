using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class UserTypeRole: BaseEntity
    {
        [ForeignKey("UserType")]
        public Guid UserTypeId { get; set; }
        public virtual UserType UserType { get; set; }
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
