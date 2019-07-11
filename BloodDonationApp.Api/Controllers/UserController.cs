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
        private readonly ISMSService smsService;
        private readonly IEmailService emailService;
        public UserController(UserManager<User> userManager, ApplicationDbContext context, ISMSService smsService, IEmailService emailService)
        {
            this.userManager = userManager;
            this.context = context;
            this.smsService = smsService;
            this.emailService = emailService;
        }

        [Route("SignUp")]
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

        [Route("VerifyMobile")]
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

                var changePhoneNumber = await userManager.SetPhoneNumberAsync(user, PhoneNumber);

                if (!changePhoneNumber.Succeeded)
                {
                    return new APIModel<string> { Success = false, Data = "Unable to set/change Phone Number !", Message = String.Join(Environment.NewLine, changePhoneNumber.Errors.Select(e => e.Description).ToList()) };
                }

                var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, PhoneNumber);
                var sendSMS = smsService.SendSMS(PhoneNumber, "Dear " + user.UserName + ", Your verification code: " + token);
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

        [HttpPost]
        [Route("VerifyTokenForMobile")]
        [Authorize]
        public async Task<APIModel<string>> VerifyTokenForMobile([FromBody]TokenModel token)
        {
            try
            {
                var userId = this.GetLoggedInUserId();
                var user = await userManager.FindByIdAsync(userId.ToString());
                var verifyToken = await userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token.Token);
                if (verifyToken.Succeeded)
                {
                    return new APIModel<string> { Success = true, Data = "Success ! Your Mobile Number is Verified.", Message = "" };
                }

                return new APIModel<string> { Success = false, Data = "Sorry ! The token is invalid or has expired.", Message = String.Join(Environment.NewLine, verifyToken.Errors.Select(e => e.Description).ToList()) };
            }
            catch(Exception ex)
            {
                return new APIModel<string> { Success = false, Data = "Internal Error !", Message = ex.Message };
            }

        }

        [Route("VerifyEmail")]
        [Authorize]
        public async Task<APIModel<string>> VerifyEmail(string NewEmail = null)
        {
            try
            {
                var userId = this.GetLoggedInUserId();
                var user = await userManager.FindByIdAsync(userId.ToString());
                var EmailAddress = user.Email;
                if (NewEmail != null)
                {
                    EmailAddress = NewEmail;
                }

                var changeEmail = await userManager.SetEmailAsync(user, EmailAddress);

                if (!changeEmail.Succeeded)
                {
                    return new APIModel<string> { Success = false, Data = "Unable to set/change Email Address !", Message = String.Join(Environment.NewLine, changeEmail.Errors.Select(e=> e.Description).ToList()) };
                }

                var token = await userManager.GenerateChangeEmailTokenAsync(user, EmailAddress);
                var fullUrl = Url.Action("VerifyTokenForEmail", "User", new { id=user.Id, token }, Request.Scheme);

                var sendSMS = emailService.SendMail(EmailAddress, "Email Verification", Helper.GetVerifyEmailHtmlString(user.UserName, fullUrl), null);
                if (!sendSMS)
                {
                    return new APIModel<string> { Success = false, Data = "Unable to send email !", Message = "" };
                }

                return new APIModel<string> { Success = true, Data = "Please check your email, a verification link is sent to your email !", Message = "" };
            }
            catch (Exception ex)
            {
                return new APIModel<string> { Success = false, Data = "Internal Error !", Message = ex.Message };
            }
        }

        [Route("VerifyTokenForEmail")]
        [AllowAnonymous]
        public async Task<string> VerifyTokenForEmail(string id, string token)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                var verifyToken = await userManager.ChangeEmailAsync(user, user.Email, token);
                if (verifyToken.Succeeded)
                {
                    return "Success ! Your email address is verified.";
                }

                return "Sorry ! The link is invalid or has expired.";
            }
            catch (Exception ex)
            {
                return "Oops! Something went wrong.";
            }

        }

    }
}