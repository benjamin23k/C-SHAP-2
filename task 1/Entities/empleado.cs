using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_Entities
{

   public class Empleado : miembro_comunidad
   {
      public string rol { get; set; } = string.Empty;
      public double salario { get; set; }

   }


}




