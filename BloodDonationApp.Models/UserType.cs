using System;
using System.Collections.Generic;
using System.Text;

namespace BloodDonationApp.Models
{
    public class UserType: BaseEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
