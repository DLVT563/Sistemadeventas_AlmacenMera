using System;
using System.Collections.Generic;

namespace POSVentaMera.Models;

public class Almacen
{
    public int IdAlmacen { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public DateTime? FechaEntrada { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }
}
