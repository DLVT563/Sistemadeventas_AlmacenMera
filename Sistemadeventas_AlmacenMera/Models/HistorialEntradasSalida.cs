using System;
using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models;

public partial class HistorialEntradasSalida
{
    public int IdHistorial { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public string? TipoMovimiento { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public string? Observaciones { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
