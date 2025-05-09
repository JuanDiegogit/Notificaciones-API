using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace NotificationAPI.Servicios
{
    public class ServicioAutenticacion
    {
        private readonly string _key;
        private readonly string _iss;
        private readonly string _aud;

        public ServicioAutenticacion(IConfiguration config)
        {
            _key = config["Jwt:Key"]
                 ?? throw new ArgumentNullException("Jwt:Key");
            _iss = config["Jwt:Issuer"];
            _aud = config["Jwt:Audience"];
        }

        public string GenerarToken(int usuarioId, string rol)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new Claim(ClaimTypes.Role, rol)
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256
            );

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _iss,
                Audience = _aud,
                SigningCredentials = creds
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDesc);
            return handler.WriteToken(token);
        }
    }
}
