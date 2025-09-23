using System;
using System.Collections.Generic;

namespace POSVentaMera.Models;

public class PagosFiado
{
    public int IdPagoFiado { get; set; }

    public int? IdFiado { get; set; }

    public decimal MontoPago { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? MetodoPago { get; set; }

    public virtual Fiado? IdFiadoNavigation { get; set; }
}
