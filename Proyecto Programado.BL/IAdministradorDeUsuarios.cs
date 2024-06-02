using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Proyecto_Programado.BL
{
    public interface IAdministradorDeUsuarios
    {

        int RegistrarUsuario(string nombre, string correo, string clave);
        void EnvieCorreoElectronico(string destinatario, string asunto, string contenido);
        bool VerifiqueCredenciales(string nombre, string clave);
    }
}
