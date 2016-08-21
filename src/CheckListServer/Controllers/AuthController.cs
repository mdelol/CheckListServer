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
      var identity = new MyIdentity("SuperSecureLogin", firstOrDefault);
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

  public class MyIdentity : ClaimsIdentity
  {
    public MyIdentity(string authenticationType, User user) : base(authenticationType)
    {
      User = user;
    }

      public User User { get; }
  }
}