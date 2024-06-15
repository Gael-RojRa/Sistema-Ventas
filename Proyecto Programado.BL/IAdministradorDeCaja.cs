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
        public void AbraLaCaja(Model.AperturaDeCaja laCaja, string elNombreDeUsuario);
        public void CierreLaCaja(int elId);
        public AperturaDeCaja ObtengaLaCajaAbierta(string elNombreDeUsuario);

    }
}
