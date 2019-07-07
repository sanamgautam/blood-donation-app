using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class RoleNavigation: BaseEntity
    {
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        [ForeignKey("Navigation")]
        public Guid NavigationId { get; set; }
        public virtual Navigation Navigation { get; set; }
    }
}
