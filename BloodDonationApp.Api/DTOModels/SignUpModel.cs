using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodDonationApp.Api
{
    public class SignUpModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Full Name !")]
        public string FullName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Address !")]
        public string Address { get; set; }
        [EmailAddress(ErrorMessage = "Please enter valid Email Address!"), Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Email Address !")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Mobile Number !"),RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter 10 digit mobile number")]
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string Profession { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Username !")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Password !")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be 8 to 20 characters long !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match !")]
        public string ConfirmPassword { get; set; }
    }
}
