using API.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Identity.Infrastructure
{
    public class AuthService
    {
        private readonly string SecretKey;
        private readonly string Issuer;
        private readonly int ExpireSpan;
        private readonly UserManager<AppUser> userManager;

        public AuthService(
            IConfiguration configuration,
            UserManager<AppUser> userManager
            )
        {
            SecretKey = configuration.GetValue<string>("Jwt:SecretKey");
            Issuer = configuration.GetValue<string>("Jwt:Issuer");
            ExpireSpan = configuration.GetValue<int>("Jwt:ExpireSpan");
            this.userManager = userManager;
        }

        public async Task<string> GenerateJwtAsync(AppUser user)
        {
            if (user == null)
                return null;

            var roles = await userManager.GetRolesAsync(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (roles != null & roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var token = new JwtSecurityToken(Issuer,
                Issuer,
                claims,
                expires: DateTime.Now.AddMinutes(ExpireSpan),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
