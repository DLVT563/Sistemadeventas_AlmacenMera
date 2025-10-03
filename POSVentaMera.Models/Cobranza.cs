using System;
using System.ComponentModel.DataAnnotations;

namespace POSVentaMera.Models
{
    public class Cobranza
    {
        public int IdCobranza { get; set; }

        [Display(Name = "Contribuyente")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un contribuyente válido.")]
        public int IdContribuyente { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Servicio / Concepto")]
        public string Servicio { get; set; }

        [Required]
        [Range(0, 999999)]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Periodo (Mes)")]
        public DateTime Periodo { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [Display(Name = "¿Es recurrente?")]
        public bool EsRecurrente { get; set; } = true;

        [Display(Name = "Fecha de Pago")]
        [DataType(DataType.Date)]
        public DateTime? FechaPago { get; set; }

        [StringLength(500)]
        public string? Notas { get; set; }

        public Contribuyente Contribuyente { get; set; }
    }
}
