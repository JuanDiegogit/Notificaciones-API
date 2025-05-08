using System;

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
	public Usuario Usuario { get; set; }
}
