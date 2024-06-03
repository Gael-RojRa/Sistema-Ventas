using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programado.UI.ViewModels
{
    public class UsuarioVM
    {

        public string Nombre { get; set; }


        [DataType(DataType.Password)]
        public string Clave { get; set; }

        public string ConfirmarClave { get; set; }

        [DataType(DataType.EmailAddress)]
        public string correoElectronico { get; set; }


    }
}
