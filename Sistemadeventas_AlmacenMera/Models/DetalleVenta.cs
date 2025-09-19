using System;
using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models;

public class DetalleVenta
{
    public int IdDetalle { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
