using System;
using System.Collections.Generic;

namespace POSVentaMera.Models;

public class Fiado
{
    public int IdFiado { get; set; }

    public int? IdVenta { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal? MontoPagado { get; set; }

    public decimal SaldoPendiente { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public string? Estado { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }

    public virtual ICollection<PagosFiado> PagosFiados { get; set; } = new List<PagosFiado>();
}
