using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class Navigation: BaseEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        [ForeignKey("ParentNavigation")]
        public Guid? ParentNavigationId { get; set; }
        public virtual Navigation ParentNavigation { get; set; }
        public int DisplayOrder { get; set; }
    }
}
