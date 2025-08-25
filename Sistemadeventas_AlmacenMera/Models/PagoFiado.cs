namespace Sistemadeventas_AlmacenMera.Models
{
    public class PagoFiado
    {
        public int IdPagoFiado { get; set; }
        public int IdFiado { get; set; }
        public decimal MontoPago { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Now;
        public string MetodoPago { get; set; }

        public Fiado Fiado { get; set; }
    }

}
