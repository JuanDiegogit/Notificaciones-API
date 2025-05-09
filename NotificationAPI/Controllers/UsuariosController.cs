using System;
using NotificationAPI.Data;
using NotificationAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NotificationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            if (usuarios == null || usuarios.Count == 0)
            {
                return NotFound("No se encontraron usuarios.");
            }

            return usuarios;
        }

        // GET 
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound($"No se encontró el usuario con ID {id}.");
            }

            return usuario;
        }

        // POST 
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("El objeto usuario no puede ser nulo.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT 
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            if (usuario == null || id != usuario.Id)
            {
                return BadRequest("El ID no coincide o los datos proporcionados son inválidos.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == id))
                {
                    return NotFound($"No se encontró el usuario con ID {id}.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE 
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound($"No se encontró el usuario con ID {id}.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
