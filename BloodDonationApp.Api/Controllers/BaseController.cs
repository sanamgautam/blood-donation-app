using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public Guid GetLoggedInUserId()
        {
            if (this.ControllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity;
                return Guid.Parse(identity.Claims.Where(c => c.Type == "UserId").First().Value);
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}