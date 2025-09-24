﻿using System;
using System.Collections.Generic;

namespace POSVentaMera.Models;

public class HistorialVentasUsuario
{
    public int IdHistorialVenta { get; set; }

    public int? IdVenta { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public DateTime? FechaVenta { get; set; }

    public decimal TotalVenta { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
