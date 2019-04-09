using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoginController(LibraryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> LoginUsers(Users user)
        {
            Users userContext = _context.Users.Where((Users u) => u.Login == user.Login && u.Pwd == user.Pwd).FirstOrDefault<Users>();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("une phrase secrète d'une longueur > 16 caractères"));
            var claims = new Claim[] {
                new Claim(ClaimTypes.Name, userContext.Login),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Svc", "Informatique"),
                new Claim(JwtRegisteredClaimNames.Exp, new
                       DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new
                       DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);


            return Ok(jwtToken);

        }
    }
}