using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public interface IAdministradorDeVentas
    {
        void RegistreVenta(Model.Venta venta);
        List<Inventario> ObtenLaListaDeInventarios();
    }
}
