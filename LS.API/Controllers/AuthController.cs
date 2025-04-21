using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LS.API.Controllers
{
    // Controller for handling authentication operations (JWT token generation).
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config) => _config = config;

        // Endpoint to generate a new JWT token.
        [HttpGet("token")]
        public IActionResult GetToken()
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, "some-user-id"),
            new Claim(JwtRegisteredClaimNames.Email, "a.karapetyan.dev@gmail.com"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: now.AddHours(1),
                signingCredentials: credentials
            );
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

}
