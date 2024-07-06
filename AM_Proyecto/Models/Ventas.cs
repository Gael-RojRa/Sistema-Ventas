using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_Proyecto.Models
{
    public class Venta
    {
        public DateTime Fecha { get; set; }
        public string TipoPago { get; set; } // "Efectivo", "Tarjeta", "SinpeMovil"
        public decimal Monto { get; set; }
    }
}
