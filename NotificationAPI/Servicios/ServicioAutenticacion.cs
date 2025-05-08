using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace NotificationAPI.Servicios
{
    public class ServicioAutenticacion
    {
        private readonly string _secretoJwt = "lX8bJ/m/oR7Tu+j45vdi39My5afiA1A9zjjQrMCY4OF70KsJLQe8oAFGaiqJSqiLksPku7n2EPGX9V+nmyTmyA==";

        // Genera un token para un usuario específico
        public string GenerarToken(int usuarioId, string rol)
        {
            var manejadorToken = new JwtSecurityTokenHandler();
            var clave = Encoding.UTF8.GetBytes(_secretoJwt);

            var descriptorToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim("usuarioId", usuarioId.ToString()),
            new Claim(ClaimTypes.Role, rol) // Aquí agregamos el rol al token
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(clave),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = manejadorToken.CreateToken(descriptorToken);
            return manejadorToken.WriteToken(token);
        }
    }
}
