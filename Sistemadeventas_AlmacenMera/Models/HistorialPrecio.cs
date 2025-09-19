using System;
using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models;

public class HistorialPrecio
{
    public int IdPrecio { get; set; }

    public int? IdProducto { get; set; }

    public decimal Precio { get; set; }

    public DateTime? FechaCambio { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }
}
