using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public enum EstadoVenta
    {
        [Display(Name = "En proceso")]
        EnProceso = 1,
        Terminada = 2
    }
}
