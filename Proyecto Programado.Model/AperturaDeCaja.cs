using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class AperturaDeCaja
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime FechaDeInicio { get; set; }
        public DateTime? FechaDeCierre { get; set; }
        public string Observaciones { get; set; }
        public int Estado { get; set; }
    }
}
