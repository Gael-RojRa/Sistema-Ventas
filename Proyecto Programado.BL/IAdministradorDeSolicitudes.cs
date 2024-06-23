using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public interface IAdministradorDeSolicitudes
    {
        public List<SolicitudRegistro> ObtengaLaListaDeSolicitudes();
        public bool SoliciteRegistro(string nombreUsuario, string email, string clave);
        public void NotificarAdministrador(SolicitudRegistro solicitud);
        public void EnvieElCorreoElectronico(string elDestinatario, string elAsunto, string elContenido);
        public void Active(int id);
        public Model.SolicitudRegistro ObtengaLaSolicitud(int id);
        public List<Model.SolicitudRegistro> ObtengaLaLista();
        public List<Model.SolicitudRegistro> ObtengaLaListaDePendientes();

        public bool AprobarSolicitud(int solicitudId);

    }
}
