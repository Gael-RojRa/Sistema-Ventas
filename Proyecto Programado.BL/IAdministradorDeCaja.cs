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
        AperturaDeCaja ObtenerApertura();
        void AbrirCaja(string Userid);

        //void CerrarCaja();

    }
}
