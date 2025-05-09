using Microsoft.AspNetCore.Mvc;
using NotificationAPI.Data;
using NotificationAPI.Models;
using NotificationAPI.Servicios;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;            // IEnumerable<T>
using System.Threading.Tasks;                // Task<>
using Microsoft.AspNetCore.Authorization;    // [Authorize]


namespace NotificationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorAutenticacion : ControllerBase
    {
        private readonly AppDbContext _contexto;
        private readonly ServicioAutenticacion _servicioAutenticacion;

        public ControladorAutenticacion(AppDbContext contexto, ServicioAutenticacion servicioAutenticacion)
        {
            _contexto = contexto;
            _servicioAutenticacion = servicioAutenticacion;
        }

        // POST
        [HttpPost("GenerarToken")]
        public async Task<ActionResult<string>> GenerarToken(int usuarioId)
        {
            // Validar si el usuario existe
            var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
            if (usuario == null)
            {
                return NotFound($"No se encontró el usuario con ID {usuarioId}.");
            }

            var token = _servicioAutenticacion.GenerarToken(usuarioId, usuario.Rol); // Pasamos también el rol

            return Ok(new { Token = token });
        }
    }
}
