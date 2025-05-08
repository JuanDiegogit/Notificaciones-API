using Microsoft.EntityFrameworkCore;
using NotificationAPI.Models;

namespace NotificationAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Metodo AlCrearModelo: aqui se configuran relaciones y restricciones del modelo.

            // Relacion Usuario - Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Usuario)
                .WithMany(u => u.Notificaciones)
                .HasForeignKey(n => n.UsuarioId);

            // Relacion Usuario - Pedido
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);
        }

    }
}
