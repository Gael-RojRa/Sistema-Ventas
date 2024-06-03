using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programado.UI.ViewModels
{
    public class CambioClaveVM
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ClaveNueva { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("ClaveNueva", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarClaveNueva { get; set; }
    }
}
