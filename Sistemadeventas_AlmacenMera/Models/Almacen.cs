using System;
using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models;

public class Almacen
{
    public int IdAlmacen { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public DateTime? FechaEntrada { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }
}
