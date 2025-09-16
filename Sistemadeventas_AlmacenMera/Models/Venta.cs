using System;
using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaVenta { get; set; }

    public decimal Total { get; set; }

    public string? Estado { get; set; }

    public string? TipoVenta { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<Fiado> Fiados { get; set; } = new List<Fiado>();

    public virtual ICollection<HistorialVentasUsuario> HistorialVentasUsuarios { get; set; } = new List<HistorialVentasUsuario>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
