using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodDonationApp.Api
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Username !")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Please enter Password !")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be 8 to 20 characters long !")]
        public string Password { get; set; }
    }
}
