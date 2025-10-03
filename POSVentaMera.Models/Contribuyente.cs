using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POSVentaMera.Models
{
    public class Contribuyente
    {
        public int IdContribuyente { get; set; }

        [Required]
        [StringLength(50)]
        public string Estatus { get; set; } = "Activo";

        [Required]
        [StringLength(20)]
        public string Ruc { get; set; }

        [Required]
        [StringLength(200)]
        public string NombreContribuyente { get; set; }

        [Display(Name = "P. Electrónica")]
        [StringLength(100)]
        public string? PlataformaElectronica { get; set; }

        [Display(Name = "Tipo Contribuyente")]
        [StringLength(100)]
        public string? TipoContribuyente { get; set; }

        [Display(Name = "Régimen Tributario")]
        [StringLength(150)]
        public string? RegimenTributario { get; set; }

        [Display(Name = "Régimen Laboral")]
        [StringLength(150)]
        public string? RegimenLaboral { get; set; }

        [Display(Name = "Usuario SOL")]
        [StringLength(100)]
        public string? IdentificadorSol { get; set; }

        [Display(Name = "Clave SOL")]
        [StringLength(100)]
        public string? ClaveSol { get; set; }

        [Display(Name = "Usuario AFPNet")]
        [StringLength(100)]
        public string? IdentificadorAfpenet { get; set; }

        [Display(Name = "Clave AFPNet")]
        [StringLength(100)]
        public string? ClaveAfpenet { get; set; }

        [Display(Name = "Usuario BN")]
        [StringLength(100)]
        public string? IdentificadorBn { get; set; }

        [Display(Name = "Clave BN")]
        [StringLength(100)]
        public string? ClaveBn { get; set; }

        [StringLength(200)]
        public string? Contacto { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [Display(Name = "Observaciones Internas")]
        [StringLength(500)]
        public string? ObservacionesInternas { get; set; }

        public ICollection<Cobranza> Cobranzas { get; set; } = new List<Cobranza>();
    }
}
