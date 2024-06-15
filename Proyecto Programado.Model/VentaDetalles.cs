using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class VentaDetalles
    {

        public int Id { get; set; }
        public int Id_Venta { get; set; }
        public int Id_Inventario { get; set; }
        public int Cantidad { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Precio { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Monto { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal MontoDescuento { get; set; }
    }
}
