using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

       
       
        [DisplayName("Nombre del Cliente")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string NombreCliente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [DisplayName("Tipo De Pago")]
        public TipoDePago TipoDePago { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Total { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }

        [Required]
        [DisplayName("Porcentaje de Descuento")]
        public int PorcentajeDescuento { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayName("Monto de Descuento")]
        public decimal MontoDescuento { get; set; }

        [Required]
        public EstadoVenta Estado { get; set; }

        [Required]
        public int IdAperturaDeCaja { get; set; }


    }
}
