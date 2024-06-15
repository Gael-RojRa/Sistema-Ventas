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

        public AdministradorDeVenta(DBContexto contexto)
        {
            ElContexto = contexto;
        }

        public void ActualiceLaCantidadDeInventario(int cantidadVendida)
        {
    
        }

        public void AgregueDetalleVenta(VentaDetalles nuevoDetalleVenta)
        {
            ElContexto.VentaDetalles.Add(nuevoDetalleVenta);
            ElContexto.SaveChanges();   
        }

        public string ObtengaNombreDeVenta(int idVenta)
        {
            Venta laVenta;
            laVenta = ElContexto.Ventas.Find(idVenta);
            string nombre;
            nombre = laVenta.NombreCliente;

            return nombre;
        }

        public int AgregueVenta(Venta laNuevaVenta)
        {
            ElContexto.Ventas.Add(laNuevaVenta);
            ElContexto.SaveChanges();

            int idAsignado = laNuevaVenta.Id;

            return idAsignado;
        }

        public Inventario ObtengaElInventario(int id)
        {
            Inventario elInventario = ElContexto.Inventarios.Find(id);

            return elInventario;
        }

        public List<VentaDetalles> ObtengaLosItemsDeUnaVenta(int idVenta)
        {
            var listaDeDetalles = ElContexto.VentaDetalles.Where(d => d.Id_Venta == idVenta).ToList();

            return listaDeDetalles;
        }

        public List<Inventario> ObtenLaListaDeInventarios()
        {

            var laListaDeInventarios = ElContexto.Inventarios.ToList();

            return laListaDeInventarios;
        }

        public List<Venta> ObtenLaListaDeVentas()
        {

            var laListaDeVentas = ElContexto.Ventas.ToList();

            return laListaDeVentas;
        }


        public int ObtenerIdCajaAbierta(string nombreUsuario)
        {

            AperturaDeCaja laCaja =  ElContexto.AperturasDeCaja.FirstOrDefault(c => c.UserId == nombreUsuario && c.Estado == EstadoCajas.Abierta);
            int idCaja = laCaja.Id;
            return idCaja;
        }

        public decimal ObtengaElPrecioDelInventario(int id)
        {
            Inventario elInventario = ElContexto.Inventarios.Find(id);
            decimal precio = elInventario.Precio;
            return precio;
        }

        public void ActualiceVenta(int id, Venta ventaActualizada)
        {
            Venta ventaOriginal = ElContexto.Ventas.Find(id);
            ventaOriginal.SubTotal = ventaActualizada.SubTotal;
            ventaOriginal.Total = ventaActualizada.Total;
            ventaOriginal.MontoDescuento = ventaActualizada.MontoDescuento;
            ventaOriginal.PorcentajeDescuento = ventaActualizada.PorcentajeDescuento;

            ElContexto.Ventas.Update(ventaOriginal);
            ElContexto.SaveChanges();
        }

        public void ActualiceElTotalEnElIndexDeVentas(int id, VentaDetalles nuevoDetalle)
        {
            decimal sumatoriaDelMontoDeDetalles = 0;
            VentaDetalles detalleActual = ElContexto.VentaDetalles.Find(id);

            if (detalleActual != null)
            {
                sumatoriaDelMontoDeDetalles = ElContexto.VentaDetalles
                    .Where(d => d.Id_Venta == detalleActual.Id_Venta)
                    .Sum(d => d.Monto);
            }

            Venta ventaAModificar = ElContexto.Ventas.Find(id);
            ventaAModificar.SubTotal = sumatoriaDelMontoDeDetalles;
            ventaAModificar.Total = sumatoriaDelMontoDeDetalles;

            ElContexto.Ventas.Update(ventaAModificar);
            ElContexto.SaveChanges();
        }

        public void ApliqueElDescuento(int porcentajeDescuento, int id)
        {
            decimal porcentajeDecimal = porcentajeDescuento / 100.0m;

            ElContexto.Database.ExecuteSqlRaw(
                "UPDATE VentaDetalles " +
                "SET MontoDescuento = Monto * {0}, " +
                "MontoFinal = Monto - (Monto * {0}) " +
                "WHERE Id_Venta = {1}",
                porcentajeDecimal, id);

            ElContexto.Database.ExecuteSqlRaw(
                "UPDATE Ventas " +
                "SET PorcentajeDesCuento = {2}, " +
                "MontoDescuento = SubTotal * {0}, " +
                "Total = SubTotal - (SubTotal * {0}) " +
                "WHERE Id = {1}",
                porcentajeDecimal, id, porcentajeDescuento);
        }
        public void ActualiceLaCantidadDeInventario(int cantidadVendida, int idInventario)
        {
            Inventario inventario = ElContexto.Inventarios.Find(idInventario);
            if (inventario != null)
            {
                inventario.Cantidad -= cantidadVendida;
                ElContexto.Inventarios.Update(inventario);
                ElContexto.SaveChanges();
            }
        }
        public Venta ObtengaVentaPorId(int idVenta)
        {
            return ElContexto.Ventas.Find(idVenta);
        }

        public void EliminarVenta(int id)
        {
            var itemAEliminar = ElContexto.VentaDetalles.FirstOrDefault(item => item.Id == id);

            ElContexto.VentaDetalles.Remove(itemAEliminar);
            ElContexto.SaveChanges();

        }

    }
}
