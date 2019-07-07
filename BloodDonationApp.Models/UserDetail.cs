using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class UserDetail : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [ForeignKey("BloodGroup")]
        public Guid BloodGroupId { get; set; }
        public virtual BloodGroup BloodGroup { get; set; }
    }
}
