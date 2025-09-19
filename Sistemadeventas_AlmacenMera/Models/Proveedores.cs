using System;
using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models;

public class Proveedores
{
    public int IdProveedor { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
