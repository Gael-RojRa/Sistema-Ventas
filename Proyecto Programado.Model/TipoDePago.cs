using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public enum TipoDePago
    {
        [Display(Name = "Por Definir")]
        PorDefinir = 0,
        Efectivo = 1,
        Tarjeta = 2,
        SinpeMovil = 3
    }
}
