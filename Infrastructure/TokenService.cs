using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class TokenService(IOptions<AppSettings> options) : ITokenService
    {
        public string GenereToken(string correo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var llave = Encoding.ASCII.GetBytes(options.Value.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                         new(ClaimTypes.NameIdentifier,correo.Trim()),
                        }
                        ),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                //#region Registrar Error

                //var error = Generadores.Error(_servicio, Generadores.ObtenerImprimirNombreDelMetodo(), HelperUsuario.ObtenerToken, ex);
                //await _mongo.RegistrarError(error);
                //#endregion
                throw;
            }
        }
    }
}
