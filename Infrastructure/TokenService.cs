using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure
{
    public class TokenService(IOptions<AppSettings> options) : ITokenService
    {
        public string GenereToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                byte[] key = Encoding.ASCII.GetBytes(options.Value.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        claims: [new(ClaimTypes.NameIdentifier,email.Trim()), ]
                        ),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ) { throw; }
        }
    }
}
