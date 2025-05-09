using Microsoft.AspNetCore.Mvc;
using NotificationAPI.Data;
using NotificationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;            
using System.Threading.Tasks;                
using Microsoft.AspNetCore.Authorization;    


namespace NotificationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        // GET Devuelve todas las notificaciones.

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            var notificaciones = await _context.Notifications
                .Include(n => n.Usuario)
                .ToListAsync();

            if (notificaciones == null || notificaciones.Count == 0)
            {
                return NotFound("No se encontraron notificaciones.");
            }

            return notificaciones;
        }

        // GET Devuelve una notificación específica
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var notification = await _context.Notifications
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (notification == null)
            {
                return NotFound($"No se encontró la notificación con ID {id}.");
            }

            return notification;
        }

        // POST
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Notification>> CreateNotification(Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("El objeto notificación no puede ser nulo.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        // PUT
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, Notification notification)
        {
            if (notification == null || id != notification.Id)
            {
                return BadRequest("El ID no coincide o los datos proporcionados son inválidos.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Notifications.Any(e => e.Id == id))
                {
                    return NotFound($"No se encontró la notificación con ID {id}.");
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
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound($"No se encontró la notificación con ID {id}.");
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
