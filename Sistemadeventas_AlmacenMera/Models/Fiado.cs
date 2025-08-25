namespace Sistemadeventas_AlmacenMera.Models
{
    public class Fiado
    {
        public int IdFiado { get; set; }
        public int IdVenta { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; } = 0.00m;
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime? FechaVencimiento { get; set; }
        public string Estado { get; set; } = "activo";

        public Venta Venta { get; set; }
        public ICollection<PagoFiado> PagosFiado { get; set; }
    }

}
