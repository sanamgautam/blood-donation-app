using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class Verification
    {
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string VerificationType { get; set; }
        public string VerificationCode { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
