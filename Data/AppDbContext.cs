using Microsoft.EntityFrameworkCore;
using ShipmentsApp.Models;

namespace ShipmentsApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Representa la tabla Shipments en la base de datos
    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // El TrackingNumber no puede repetirse
        builder.Entity<Shipment>()
            .HasIndex(s => s.TrackingNumber)
            .IsUnique();
    }
    public DbSet<Usuario> Usuarios { get; set; }
}