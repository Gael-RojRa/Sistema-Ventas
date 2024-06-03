using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class Inventario
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public Categoria Categoria { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

    }
}
