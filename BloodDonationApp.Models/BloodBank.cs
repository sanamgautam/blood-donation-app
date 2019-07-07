using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class BloodBank: BaseEntity
    {
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
    }
}
