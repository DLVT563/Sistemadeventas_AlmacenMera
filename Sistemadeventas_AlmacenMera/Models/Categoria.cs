using System;
using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models;

public class Categoria
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
