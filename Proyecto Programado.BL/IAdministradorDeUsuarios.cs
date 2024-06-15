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

        bool RegistreElUsuario(string elNombre, string elCorreo, string laClave);
        void EnvieElCorreoElectronico(string elDestinatario, string elAsunto, string elContenido);
        bool VerifiqueCredenciales(string elNombre, string laClave);
        Usuario ObtengaElUsuarioPorNombre(string elNombre);
        Rol ObtengaElRolDelUsuario(string elNombre);
        void CambieLaClave(Usuario elUsuario, string laClaveNueva);
    }
}
