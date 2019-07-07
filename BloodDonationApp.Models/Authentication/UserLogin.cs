using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class UserLogin: IdentityUserLogin<Guid>
    {
    }
}
