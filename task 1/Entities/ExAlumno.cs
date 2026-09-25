using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_Entities
{
   

    public class ExAlumno : miembro_comunidad
    {
        string añoGraduacion { get; set; } = string.Empty;

        string titulo { get; set; } = string.Empty;
    }
}
