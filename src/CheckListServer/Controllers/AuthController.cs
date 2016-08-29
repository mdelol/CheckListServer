using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Contexts;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Task = DAL.Models.Task;

namespace CheckListServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ProjectContext _context;

        public AuthController(ProjectContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public string Login([FromBody] UserInfo value)
        {
            var firstOrDefault =
              _context.Users.FirstOrDefault(x => x.UserName == value.Name && x.PasswordHash == value.Password);
            if (firstOrDefault == null)
            {
                return "false";
            }
            var identity = new ClaimsIdentity("SuperSecureLogin");
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, value.Name, ClaimValueTypes.String, "Issuer") };
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

        [HttpPost]
        [Route("/api/users/create")]
        [AllowAnonymous]
        public string Create([FromBody] UserInfo value)
        {
            _context.Users.Add(new User { UserName = value.Name, PasswordHash = value.Password, Tasks =  new List<Task>()});
            _context.SaveChanges();
            return "true";
        }
    }
}