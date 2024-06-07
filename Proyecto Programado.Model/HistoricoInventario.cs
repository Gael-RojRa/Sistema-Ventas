using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class HistoricoInventario
    {
        public int Id { get; set; }
        public int IdInventario {  get; set; }
        public String nombreUsuario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool EsCreacion { get; set; }

    }
}
