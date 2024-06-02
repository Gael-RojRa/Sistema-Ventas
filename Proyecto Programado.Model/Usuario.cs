using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programado.Model
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        [DataType(DataType.EmailAddress)]
        public string correoElectronico { get; set; }
        public Rol rol { get; set; }
    }
}
