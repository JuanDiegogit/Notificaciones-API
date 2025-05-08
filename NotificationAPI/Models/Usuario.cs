using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationAPI.Models
{
	public class Usuario
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[Required]
		[MaxLength(150)]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string Rol { get; set; } = "Cliente";

		public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

		// Relaciones
		public ICollection<Notification> Notificaciones { get; set; } = new List<Notification>();
		public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
	}
}
