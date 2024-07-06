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

        List<Inventario> ObtengaLaListaDeInventarios();

        List<Venta> ObtengaLaListaDeVentas();
        Inventario ObtengaElInventario(int elId);

        int AgregueLaVenta(Venta laNuevaVenta);
        void AgregueDetalleVenta(VentaDetalles elNuevoDetalleDeVenta);

        List<VentaDetalles> ObtengaLosItemsDeUnaVenta(int elIdVenta);

        int ObtengaElIdDeLaCajaAbierta(string elNombreUsuario);

        decimal ObtengaElPrecioDelInventario(int elId);

        void ActualiceLaVenta(int elId, Venta laVentaActualizada);

        string ObtengaElNombreDeVenta(int elIdDeVenta);

        void ActualiceElTotalEnElIndexDeVentas(int elId);

        void ApliqueElDescuento(int elPorcentajeDescuento, int elId);

        void ActualiceLaCantidadDeInventario(int laCantidadVendida, int elIdInventario);

        Venta ObtengaVentaPorId(int elIdVenta);

        VentaDetalles ObtengaVentaDetallePorId(int elIdVentaDeDetalle);

        void RestaureLaCantidadDelItemEliminado(int laCantidadDevuelta, int elIdInventario);

        void ElimineLaVenta(int elId);

        public bool VerifiqueLaCajaAbierta(string elNombreUsuario);
    }

}
