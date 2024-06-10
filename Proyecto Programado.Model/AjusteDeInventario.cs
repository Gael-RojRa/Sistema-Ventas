using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class AjusteDeInventario
    {
        public int Id { get; set; }
        public int Id_Inventario { get; set; }
        public int CantidadActual { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser un número positivo.")]
        public int Ajuste { get; set; }
        public TipoAjuste Tipo { get; set; }
        public string Observaciones { get; set; }
        public string UserId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
