using Proyecto_Programado.DA;
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
        List<Inventario> ObtenLaListaDeInventarios();

        List<Venta> ObtenLaListaDeVentas();
        Inventario ObtengaElInventario(int id);

        void ActualiceLaCantidadDeInventario(int cantidadVendida);
        int AgregueVenta(Venta laNuevaVenta);
        void AgregueDetalleVenta(VentaDetalles nuevoDetalleVenta);

        List<VentaDetalles> ObtengaLosItemsDeUnaVenta(int idVenta);

        int ObtenerIdCajaAbierta(string nombreUsuario);

        decimal ObtengaElPrecioDelInventario(int id);

        void ActualiceVenta(int id, Venta ventaActualizada);

        string ObtengaNombreDeVenta(int idVenta);

        void ActualiceElTotalEnElIndexDeVentas(int id, VentaDetalles nuevoDetalle);
    }
}
