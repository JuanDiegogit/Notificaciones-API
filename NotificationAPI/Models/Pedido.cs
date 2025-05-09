using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationAPI.Models
{
	public class Pedido
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } 

        [Required]
		public int UsuarioId { get; set; } 

        [Required]
		[MaxLength(100)]
		public string Producto { get; set; } = string.Empty;

		[Required]
		public int Cantidad { get; set; } 

        [Required]
		public decimal PrecioTotal { get; set; } 

        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

		// Navegación
		[ForeignKey("UsuarioId")]
		public Usuario Usuario { get; set; } = null!;
    }
}