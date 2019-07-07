using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodDonationApp.Data;
using BloodDonationApp.Models;
using BloodDonationApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BloodDonationApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private ApplicationDbContext context;
        private UserManager<User> userManager;
        ISMSService SMSService;
        public UserController(UserManager<User> userManager, ApplicationDbContext context, ISMSService SMSService)
        {
            this.userManager = userManager;
            this.context = context;
            this.SMSService = SMSService;
        }

        [Route("signup")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<APIModel<string>> SignUp(SignUpModel signUpModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Values.Where(v => v.ValidationState == ModelValidationState.Invalid).FirstOrDefault();
                    return new APIModel<string> { Success = false, Data = error.Errors.FirstOrDefault().ErrorMessage, Message = "" };
                }

                if(await userManager.FindByEmailAsync(signUpModel.Email)!= null)
                {
                    return new APIModel<string> { Success = false, Data = "This email is already in use ! Please enter another email.", Message = "" };
                }

                if (context.Users.Where(u=> u.PhoneNumber == signUpModel.MobileNumber && u.PhoneNumberConfirmed).Any())
                {
                    return new APIModel<string> { Success = false, Data = "This Mobile Number is already in use ! Please enter another Mobile Number.", Message = "" };
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = signUpModel.Email,
                    EmailConfirmed = false,
                    PhoneNumber = signUpModel.MobileNumber,
                    PhoneNumberConfirmed = false,
                    UserName = signUpModel.Username
                };

                var result = await userManager.CreateAsync(user, signUpModel.Password);
                if (!result.Succeeded)
                {
                    string ErrorMessage = string.Empty;
                    if (result.Errors.Count() > 0)
                    {
                        ErrorMessage = result.Errors.FirstOrDefault().Description;
                    }
                    return new APIModel<string> { Success = false, Data = string.IsNullOrWhiteSpace(ErrorMessage)? "Internal Error !": ErrorMessage, Message = "" };
                }

                // define roles for user...

                return new APIModel<string> { Success = true, Data = "<h4>Registration Successfull !<h4> <h6>Please login to the app using your Username and Password.</h6>", Message = "" };
            }
            catch (Exception ex)
            {
                return new APIModel<string> { Success = false, Data = "Internal Error !", Message = ex.Message };
            }
        }

        [Route("verifymobile")]
        [Authorize]
        public async Task<APIModel<string>> VerifyMobile(string MobileNumber=null)
        {
            try
            {
                var userId = this.GetLoggedInUserId();
                var user = await userManager.FindByIdAsync(userId.ToString());
                var PhoneNumber = user.PhoneNumber;
                if (MobileNumber != null)
                {
                    PhoneNumber = MobileNumber;
                }

                var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, PhoneNumber);
                var sendSMS = SMSService.SendSMSAsync(PhoneNumber, "Dear " + user.UserName + ", Your verification code: " + token);
                if (!sendSMS)
                {
                    return new APIModel<string> { Success = false, Data = "Unable to send token via SMS !", Message = "" };
                }

                return new APIModel<string> { Success = true, Data = "A token is sent to your Mobile Number !", Message = "" };
            }
            catch (Exception ex)
            {
                return new APIModel<string> { Success = false, Data = "Internal Error !", Message = ex.Message };
            }
        }
    }
}