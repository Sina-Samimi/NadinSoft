using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Utilities
{
    public class GenerateTokenForUser
    {
        private readonly IConfiguration configuration;

        public GenerateTokenForUser(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Getoken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId",user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            string key = configuration["JWtConfig:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWtConfig:issuer"],
                audience: configuration["JWtConfig:audience"],
                expires: DateTime.Now.AddMinutes(int.Parse(configuration["JWtConfig:expires"])),
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: credentials
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken.ToString();
        }
    }
}
