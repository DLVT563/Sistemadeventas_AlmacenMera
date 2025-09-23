using System;
using System.Collections.Generic;

namespace POSVentaMera.Models;

public class Productos
{
    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? CodigoBarras { get; set; }

    public string? FotoPath { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdCategoria { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public virtual ICollection<Almacen> Almacens { get; set; } = new List<Almacen>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<HistorialEntradasSalida> HistorialEntradasSalida { get; set; } = new List<HistorialEntradasSalida>();

    public virtual ICollection<HistorialPrecio> HistorialPrecios { get; set; } = new List<HistorialPrecio>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual Proveedores? IdProveedorNavigation { get; set; }
}
