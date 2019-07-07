using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BloodDonationApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BloodDonationApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<User> userManager;
        public AuthController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await userManager.FindByNameAsync(loginModel.UserName);
            if(user!=null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("UserId", user.Id.ToString())
                };

                var roles = await userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    var roleClaim = new Claim(ClaimTypes.Role, role);
                    claims.Add(roleClaim);
                }

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                var token = new JwtSecurityToken(
                        issuer: "http://blooddonationapp.com",
                        audience: "http://blooddonationapp.com",
                        expires: DateTime.Now.AddHours(2),
                        claims: claims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                        );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires = token.ValidTo
                });
            }

            return Unauthorized();
        }
    }
}