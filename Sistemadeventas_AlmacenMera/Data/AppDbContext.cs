using Microsoft.EntityFrameworkCore;
using Sistemadeventas_AlmacenMera.Models;

namespace Sistemadeventas_AlmacenMera.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Fiado> Fiados { get; set; }
        public DbSet<PagosFiado> PagosFiados { get; set; }
        public DbSet<HistorialVentasUsuario> HistorialVentasUsuarios { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<HistorialPrecio> HistorialPrecios { get; set; }
        public DbSet<HistorialEntradasSalida> HistorialEntradasSalidas { get; set; }
        public DbSet<Almacen> Almacenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Contraseña).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Estado).HasMaxLength(50).HasDefaultValue("Activo");

                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(e => e.IdRolNavigation)
                      .WithMany(r => r.Usuarios)
                      .HasForeignKey(e => e.IdRol)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasData(
        new Usuario
        {
            IdUsuario = 1,
            Nombre = "Administrador",
            Email = "admin@admin.com",
            Contraseña = "admin123", 
            IdRol = 1,
            FechaCreacion = new DateTime(2025, 01, 01),
            Estado = "Activo"
        },
        new Usuario
        {
            IdUsuario = 2,
            Nombre = "Empleado",
            Email = "emp@emp.com",
            Contraseña = "123",
            IdRol = 2,
            FechaCreacion = new DateTime(2025, 01, 01),
            Estado = "Activo"
        }
    );
            });

            // Roles
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.IdRol);
                entity.Property(e => e.NombreRol).HasMaxLength(100).IsRequired();

                entity.HasData(
        new Roles { IdRol = 1, NombreRol = "Admin" },
        new Roles { IdRol = 2, NombreRol = "Empleado" }
    );
            });

            // Venta
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.IdVenta);

                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Estado).HasMaxLength(50).HasDefaultValue("Pendiente");
                entity.Property(e => e.TipoVenta).HasMaxLength(50);

                entity.Property(e => e.FechaVenta)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(v => v.IdUsuarioNavigation)
                      .WithMany(u => u.Venta)
                      .HasForeignKey(v => v.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // DetalleVenta
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(e => e.IdDetalle);

                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.IdVentaNavigation)
                      .WithMany(v => v.DetalleVenta)
                      .HasForeignKey(d => d.IdVenta)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.IdProductoNavigation)
                      .WithMany(p => p.DetalleVenta)
                      .HasForeignKey(d => d.IdProducto)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Fiado
            modelBuilder.Entity<Fiado>(entity =>
            {
                entity.HasKey(e => e.IdFiado);

                entity.Property(e => e.MontoTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MontoPagado).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SaldoPendiente).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Estado).HasMaxLength(50).HasDefaultValue("Pendiente");

                entity.Property(e => e.FechaInicio).HasDefaultValueSql("GETDATE()");

                entity.HasOne(f => f.IdVentaNavigation)
                      .WithMany(v => v.Fiados)
                      .HasForeignKey(f => f.IdVenta)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // PagosFiado
            modelBuilder.Entity<PagosFiado>(entity =>
            {
                entity.HasKey(e => e.IdPagoFiado);

                entity.Property(e => e.MontoPago).HasColumnType("decimal(18,2)");

                entity.Property(e => e.FechaPago)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(p => p.IdFiadoNavigation)
                      .WithMany(f => f.PagosFiados)
                      .HasForeignKey(p => p.IdFiado)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // HistorialVentasUsuario
            modelBuilder.Entity<HistorialVentasUsuario>(entity =>
            {
                entity.HasKey(e => e.IdHistorialVenta);

                entity.Property(e => e.TotalVenta).HasColumnType("decimal(18,2)");

                entity.Property(e => e.FechaVenta)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(h => h.IdVentaNavigation)
                      .WithMany(v => v.HistorialVentasUsuarios)
                      .HasForeignKey(h => h.IdVenta)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Proveedores
            modelBuilder.Entity<Proveedores>(entity =>
            {
                entity.HasKey(e => e.IdProveedor);

                entity.Property(e => e.NombreProveedor).HasMaxLength(150).IsRequired();

                entity.HasIndex(e => e.NombreProveedor);
            });

            // Productos
            modelBuilder.Entity<Productos>(entity =>
            {
                entity.HasKey(e => e.IdProducto);

                entity.Property(e => e.NombreProducto).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)").IsRequired();

                entity.Property(e => e.FechaCreacion)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(p => p.CodigoBarras).IsUnique();
                entity.HasIndex(p => p.NombreProducto);

                entity.HasOne(p => p.IdProveedorNavigation)
                      .WithMany(pr => pr.Productos)
                      .HasForeignKey(p => p.IdProveedor)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.IdCategoriaNavigation)
                      .WithMany(c => c.Productos)
                      .HasForeignKey(p => p.IdCategoria)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);
                entity.Property(e => e.NombreCategoria).HasMaxLength(100).IsRequired();

                entity.HasIndex(e => e.NombreCategoria);
            });

            // HistorialPrecio
            modelBuilder.Entity<HistorialPrecio>(entity =>
            {
                entity.HasKey(e => e.IdPrecio);

                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)").IsRequired();

                entity.Property(e => e.FechaCambio)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(h => h.IdProductoNavigation)
                      .WithMany(p => p.HistorialPrecios)
                      .HasForeignKey(h => h.IdProducto)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // HistorialEntradasSalida
            modelBuilder.Entity<HistorialEntradasSalida>(entity =>
            {
                entity.HasKey(e => e.IdHistorial);

                entity.Property(e => e.FechaMovimiento)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(h => h.IdProductoNavigation)
                      .WithMany(p => p.HistorialEntradasSalida)
                      .HasForeignKey(h => h.IdProducto)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Almacen
            modelBuilder.Entity<Almacen>(entity =>
            {
                entity.HasKey(e => e.IdAlmacen);

                entity.Property(e => e.FechaEntrada)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(a => a.IdProductoNavigation)
                      .WithMany(p => p.Almacens)
                      .HasForeignKey(a => a.IdProducto)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
