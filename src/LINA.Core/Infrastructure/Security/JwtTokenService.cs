using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LINA.Core.Infrastructure.Security.Abstraction;
using LINA.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LINA.Core.Infrastructure.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;

        public JwtTokenService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (nameClaim == null)
            {
                claims.Insert(0, new Claim(ClaimTypes.Name, user.UserName));
            }

            var idClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                claims.Insert(0, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            }

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(90),
                Issuer = "LINA",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("48394f394fh")), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
