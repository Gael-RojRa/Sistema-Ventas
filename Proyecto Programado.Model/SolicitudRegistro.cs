using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public class SolicitudRegistro
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }

        [Display(Name = "Correo Electronico")]
        [DataType(DataType.EmailAddress)]
        public string correoElectronico { get; set; }
        public string Clave { get; set; }
        [Display(Name = "Estado de registro")]
        public EstadoRegistro EstadoRegistro { get; set; }

        [Display(Name = "Fecha de solicitud")]
        public DateTime FechaSolicitud { get; set; }

    }

}
