using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeVenta : IAdministradorDeVentas
    {
        public DBContexto ElContexto;

        public AdministradorDeVenta(DBContexto elContexto)
        {
            ElContexto = elContexto;
        }

        public void AgregueDetalleVenta(VentaDetalles elNuevoDetalleDeVenta)
        {
            ElContexto.VentaDetalles.Add(elNuevoDetalleDeVenta);
            ElContexto.SaveChanges();   
        }

        public string ObtengaElNombreDeVenta(int elIdDeVenta)
        {
            Venta laVenta;
            laVenta = ElContexto.Ventas.Find(elIdDeVenta);
            
            string elNombre;
            elNombre = laVenta.NombreCliente;

            return elNombre;
        }

        public int AgregueLaVenta(Venta laNuevaVenta)
        {
            ElContexto.Ventas.Add(laNuevaVenta);
            ElContexto.SaveChanges();

            int elIdAsignado = laNuevaVenta.Id;

            return elIdAsignado;
        }

        public Inventario ObtengaElInventario(int elId)
        {
            Inventario elInventario = ElContexto.Inventarios.Find(elId);

            return elInventario;
        }

        public List<VentaDetalles> ObtengaLosItemsDeUnaVenta(int elIdDeVenta)
        {
            var laListaDeDetalles = ElContexto.VentaDetalles.Where(elElemento => elElemento.Id_Venta == elIdDeVenta).ToList();

            return laListaDeDetalles;
        }

        public List<Inventario> ObtengaLaListaDeInventarios()
        {

            var laListaDeInventarios = ElContexto.Inventarios.ToList();

            return laListaDeInventarios;
        }

        public List<Venta> ObtengaLaListaDeVentas()
        {

            var laListaDeVentas = ElContexto.Ventas.ToList();

            return laListaDeVentas;
        }


        public int ObtengaElIdDeLaCajaAbierta(string elNombreUsuario)
        {

            AperturaDeCaja laCaja =  ElContexto.AperturasDeCaja.FirstOrDefault(elElemento => elElemento.UserId == elNombreUsuario && elElemento.Estado == EstadoCajas.Abierta);
            int elIdCaja = laCaja.Id;
            return elIdCaja;
        }

        public decimal ObtengaElPrecioDelInventario(int id)
        {
            Inventario elInventario = ElContexto.Inventarios.Find(id);
            decimal elPrecio = elInventario.Precio;
            return elPrecio;
        }

        public void ActualiceLaVenta(int elId, Venta laVentaActualizada)
        {
            Venta ventaOriginal = ElContexto.Ventas.Find(elId);
            ventaOriginal.SubTotal = laVentaActualizada.SubTotal;
            ventaOriginal.Total = laVentaActualizada.Total;
            ventaOriginal.MontoDescuento = laVentaActualizada.MontoDescuento;
            ventaOriginal.PorcentajeDescuento = laVentaActualizada.PorcentajeDescuento;

            AperturaDeCaja laAperturaDeCaja = ElContexto.AperturasDeCaja.Find(laVentaActualizada.IdAperturaDeCaja);

            TipoDePago tipoDePago = laVentaActualizada.TipoDePago;

            if (tipoDePago == TipoDePago.Efectivo)
            {
                laAperturaDeCaja.Efectivo += laVentaActualizada.Total;
            }
            else if (tipoDePago == TipoDePago.Tarjeta)
            {
                laAperturaDeCaja.Tarjeta += laVentaActualizada.Total;
            }
            else if (tipoDePago == TipoDePago.SinpeMovil)
            {
                laAperturaDeCaja.SinpeMovil += laVentaActualizada.Total;
            }

            ElContexto.AperturasDeCaja.Update(laAperturaDeCaja);

            ElContexto.Ventas.Update(ventaOriginal);
            ElContexto.SaveChanges();
        }

        public void ActualiceElTotalEnElIndexDeVentas(int elId)
        {
            decimal laSumatoriaDelMontoDeDetalles = 0;
            VentaDetalles elDetalleActual = ElContexto.VentaDetalles.Find(elId);
            
                laSumatoriaDelMontoDeDetalles = ElContexto.VentaDetalles
                    .Where(elElemento => elElemento.Id_Venta == elDetalleActual.Id_Venta)
                    .Sum(elElemento => elElemento.Monto);
            

            Venta laVentaAModificar = ElContexto.Ventas.Find(elDetalleActual.Id_Venta);
            laVentaAModificar.SubTotal = laSumatoriaDelMontoDeDetalles;
            laVentaAModificar.Total = laSumatoriaDelMontoDeDetalles;

            ElContexto.Ventas.Update(laVentaAModificar);
            ElContexto.SaveChanges();
        }

        public void ApliqueElDescuento(int elPorcentajeDescuento, int elId)
        {
            decimal elPorcentajeDecimal = elPorcentajeDescuento / 100.0m;

            ElContexto.Database.ExecuteSqlRaw(
                "UPDATE VentaDetalles " +
                "SET MontoDescuento = Monto * {0}, " +
                "MontoFinal = Monto - (Monto * {0}) " +
                "WHERE Id_Venta = {1}",
                elPorcentajeDecimal, elId);

            ElContexto.Database.ExecuteSqlRaw(
                "UPDATE Ventas " +
                "SET PorcentajeDesCuento = {2}, " +
                "MontoDescuento = SubTotal * {0}, " +
                "Total = SubTotal - (SubTotal * {0}) " +
                "WHERE Id = {1}",
                elPorcentajeDecimal, elId, elPorcentajeDescuento);
        }
        public void ActualiceLaCantidadDeInventario(int laCantidadVendida, int elIdInventario)
        {
            Inventario elInventario = ElContexto.Inventarios.Find(elIdInventario);
            if (elInventario != null)
            {
                elInventario.Cantidad -= laCantidadVendida;
                ElContexto.Inventarios.Update(elInventario);
                ElContexto.SaveChanges();
            }
        }

        public void RestaureLaCantidadDelItemEliminado(int laCantidadDevuelta, int elIdInventario)
        {
            Inventario elInventario = ElContexto.Inventarios.Find(elIdInventario);
            if (elInventario != null)
            {
                elInventario.Cantidad += laCantidadDevuelta;
                ElContexto.Inventarios.Update(elInventario);
                ElContexto.SaveChanges();
            }
        }
        public Venta ObtengaVentaPorId(int elIdVenta)
        {
            return ElContexto.Ventas.Find(elIdVenta);
        }

        public void ElimineLaVenta(int elId)
        {
            decimal laSumatoriaDelMontoDeDetalles = 0;
            VentaDetalles elItemAEliminar = ElContexto.VentaDetalles.Find(elId);

            ElContexto.VentaDetalles.Remove(elItemAEliminar);
            ElContexto.SaveChanges();

            int idDeLaVentaDelDetalles = elItemAEliminar.Id_Venta;
            Venta laVentaParaActualizar = ElContexto.Ventas.Find(idDeLaVentaDelDetalles);

            laSumatoriaDelMontoDeDetalles = ElContexto.VentaDetalles
                .Where(elElemento => elElemento.Id_Venta == laVentaParaActualizar.Id)
                .Sum(elElemento => elElemento.Monto);

            laVentaParaActualizar.SubTotal = laSumatoriaDelMontoDeDetalles;
            decimal porcentajeDescuentoDecimal = laVentaParaActualizar.PorcentajeDescuento / 100m;
            decimal valorDescuento = laSumatoriaDelMontoDeDetalles * porcentajeDescuentoDecimal;
            laVentaParaActualizar.Total = laSumatoriaDelMontoDeDetalles - valorDescuento;
            laVentaParaActualizar.MontoDescuento = valorDescuento;

            ElContexto.Ventas.Update(laVentaParaActualizar);
            ElContexto.SaveChanges();

            if (laVentaParaActualizar.SubTotal == 0)
            {
                ElContexto.Ventas.Remove(laVentaParaActualizar);
                ElContexto.SaveChanges();
            }
        }


        public VentaDetalles ObtengaVentaDetallePorId(int elIdVentaDeDetalle)
        {
            return ElContexto.VentaDetalles.Find(elIdVentaDeDetalle);
        }

        public bool VerifiqueLaCajaAbierta(string elNombreUsuario)
        {


            AperturaDeCaja laCajaAbierta = ElContexto.AperturasDeCaja.FirstOrDefault(elElemento => elElemento.UserId == elNombreUsuario && elElemento.Estado == EstadoCajas.Abierta);
            bool laCajaEstaAbierta = (laCajaAbierta != null);
            return laCajaEstaAbierta;
        }


    }
}
