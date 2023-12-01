using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication7.Models;
using WebApplication7.Resources;

namespace WebApplication7.Controllers
{
    [EnableCors("RulesCors")]

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly VerceldbContext _dbcontext;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, VerceldbContext context)
        {
            _configuration = configuration;
            _dbcontext = context;
            
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            User oUser = await _dbcontext.Users
                .FirstOrDefaultAsync(u => u.Name == user.Name);

            if (oUser != null)
            {
                string hashPassword = Utilities.EncryptKey(user.Password);
                if (hashPassword == oUser.Password)
                {
                    var token = GenerateJwtToken(oUser);
                    return Ok(new { token = token });
                }
            }

            return Unauthorized("Credenciales inválidas.");
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

    }
