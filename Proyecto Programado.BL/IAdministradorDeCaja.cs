using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public interface IAdministradorDeCaja
    {
        public void AbrirCaja(Model.AperturaDeCaja caja, string nombreUsuario);
        public void CerrarCaja(int id);
        public AperturaDeCaja ObtenerCajaAbierta(string nombreUsuario);

    }
}
