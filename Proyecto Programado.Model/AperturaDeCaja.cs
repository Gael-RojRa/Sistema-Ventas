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
    public class AperturaDeCaja
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [DisplayName("Fecha de Inicio")]
        public DateTime FechaDeInicio { get; set; }
        public DateTime? FechaDeCierre { get; set; }
        public string? Observaciones { get; set; }
        public EstadoCajas Estado { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? Efectivo { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? Tarjeta { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? SinpeMovil { get; set; }

    }
}
