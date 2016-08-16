using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CheckListServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpPost]
        public string Login([FromBody] UserInfo value)
        {
            var identity = new ClaimsIdentity("SuperSecureLogin");
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, value.Name, ClaimValueTypes.String, "Issuer"));
            identity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(identity);
            HttpContext.Authentication.SignInAsync("Cookie", userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = false
                });

            return "true";
        }
    }

    public class UserInfo
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}

    