using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sistemadeventas_AlmacenMera.Models;

namespace Sistemadeventas_AlmacenMera.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacen> Almacens { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<Fiado> Fiados { get; set; }

    public virtual DbSet<HistorialEntradasSalida> HistorialEntradasSalidas { get; set; }

    public virtual DbSet<HistorialPrecio> HistorialPrecios { get; set; }

    public virtual DbSet<HistorialVentasUsuario> HistorialVentasUsuarios { get; set; }

    public virtual DbSet<PagosFiado> PagosFiados { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.IdAlmacen).HasName("PK__almacen__098D5D13B1E3EE71");

            entity.ToTable("almacen");

            entity.Property(e => e.IdAlmacen).HasColumnName("id_almacen");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaEntrada)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_entrada");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Almacens)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__almacen__id_prod__49C3F6B7");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__categori__CD54BC5A783E9B06");

            entity.ToTable("categorias");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__detalle___4F1332DED72C0490");

            entity.ToTable("detalle_ventas");

            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__detalle_v__id_pr__5535A963");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__detalle_v__id_ve__5441852A");
        });

        modelBuilder.Entity<Fiado>(entity =>
        {
            entity.HasKey(e => e.IdFiado).HasName("PK__fiados__420FD58DC3369324");

            entity.ToTable("fiados");

            entity.Property(e => e.IdFiado).HasColumnName("id_fiado");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasDefaultValue("activo")
                .HasColumnName("estado");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.MontoPagado)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_pagado");
            entity.Property(e => e.MontoTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_total");
            entity.Property(e => e.SaldoPendiente)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("saldo_pendiente");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Fiados)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__fiados__id_venta__5FB337D6");
        });

        modelBuilder.Entity<HistorialEntradasSalida>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__historia__76E6C5028A10B7E2");

            entity.ToTable("historial_entradas_salidas");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(10)
                .HasColumnName("tipo_movimiento");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.HistorialEntradasSalida)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__historial__id_pr__68487DD7");
        });

        modelBuilder.Entity<HistorialPrecio>(entity =>
        {
            entity.HasKey(e => e.IdPrecio).HasName("PK__historia__A70EF6EDD3220AF5");

            entity.ToTable("historial_precios");

            entity.Property(e => e.IdPrecio).HasColumnName("id_precio");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.HistorialPrecios)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__historial__id_pr__59063A47");
        });

        modelBuilder.Entity<HistorialVentasUsuario>(entity =>
        {
            entity.HasKey(e => e.IdHistorialVenta).HasName("PK__historia__B96FB70281153004");

            entity.ToTable("historial_ventas_usuario");

            entity.Property(e => e.IdHistorialVenta).HasColumnName("id_historial_venta");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_venta");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.TotalVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_venta");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.HistorialVentasUsuarios)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__historial__id_ve__6C190EBB");
        });

        modelBuilder.Entity<PagosFiado>(entity =>
        {
            entity.HasKey(e => e.IdPagoFiado).HasName("PK__pagos_fi__CDDE6AE73CCAB987");

            entity.ToTable("pagos_fiados");

            entity.Property(e => e.IdPagoFiado).HasColumnName("id_pago_fiado");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pago");
            entity.Property(e => e.IdFiado).HasColumnName("id_fiado");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.MontoPago)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_pago");

            entity.HasOne(d => d.IdFiadoNavigation).WithMany(p => p.PagosFiados)
                .HasForeignKey(d => d.IdFiado)
                .HasConstraintName("FK__pagos_fia__id_fi__6383C8BA");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__FF341C0D2E6180F9");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK__productos__id_ca__45F365D3");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__productos__id_pr__44FF419A");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__proveedo__8D3DFE28D6467724");

            entity.ToTable("proveedores");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.NombreProveedor)
                .HasMaxLength(100)
                .HasColumnName("nombre_proveedor");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__roles__6ABCB5E03EFACA1E");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<Role>().HasData(
    new Role
    {
        IdRol = 1,
        NombreRol = "Admin"
    },
    new Role
    {
        IdRol = 2,
        NombreRol = "Vendedor"
    }
);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuarios__4E3E04AD5D8725D1");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E6164A612CDC6").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasDefaultValue("activo")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__usuarios__id_rol__3D5E1FD2");
        });
        modelBuilder.Entity<Usuario>().HasData(
    new Usuario
    {
        IdUsuario = 1,
        Nombre = "Administrador",
        Email = "admin@admin.com",
        Contraseña = "123", 
        Estado = "activo",
        FechaCreacion = DateTime.Now,
        IdRol = 1
    },
    new Usuario
    {
        IdUsuario = 2,
        Nombre = "Vendedor",
        Email = "vende@vende.com",
        Contraseña = "123",
        Estado = "activo",
        FechaCreacion = DateTime.Now,
        IdRol = 2
    }
);


        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__ventas__459533BF50DF49F8");

            entity.ToTable("ventas");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasDefaultValue("pendiente")
                .HasColumnName("estado");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_venta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.TipoVenta)
                .HasMaxLength(20)
                .HasDefaultValue("contado")
                .HasColumnName("tipo_venta");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__ventas__id_usuar__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
