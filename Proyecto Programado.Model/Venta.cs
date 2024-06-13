using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

       
       
        [DisplayName("Nombre Cliente")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string NombreCliente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TipoDePago TipoDePago { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public decimal SubTotal { get; set; }

        [Required]
        public int PorcentajeDescuento { get; set; }

        [Required]
        public decimal MontoDescuento { get; set; }

        [Required]
        public EstadoVenta Estado { get; set; }

        [Required]
        public int IdAperturaDeCaja { get; set; }

        public int UsuarioId { get; set; } // Se obtiene automáticamente

    }
}
