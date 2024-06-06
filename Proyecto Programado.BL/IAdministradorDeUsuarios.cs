using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Proyecto_Programado.BL
{
    public interface IAdministradorDeUsuarios
    {

        bool RegistrarUsuario(string nombre, string correo, string clave);
        void EnvieCorreoElectronico(string destinatario, string asunto, string contenido);
        bool VerifiqueCredenciales(string nombre, string clave);
        Usuario ObtenerUsuarioPorNombre(string nombre);
        void CambiarClave(Usuario usuario, string claveNueva);
    }
}
