using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programado.Model
{
    public class Usuario
    {

        [Key]
        public string Nombre { get; set; }
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        [DataType(DataType.EmailAddress)]
        public string correoElectronico { get; set; }
        public Rol rol { get; set; }
        public int IntentosFallidos { get; set; }
        public bool EstaBloqueado { get; set; }
        public DateTime? TiempoDesbloqueo { get; set; }
    }
}
