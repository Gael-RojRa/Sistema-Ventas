using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using System.Security.Claims;


namespace Proyecto_Programado.UI.Controllers
{

    public class VentaController : Controller
    {
        public readonly IAdministradorDeVentas ElAdministrador;
        public VentaController(IAdministradorDeVentas administrador)
        {
            ElAdministrador = administrador;
        }
        // GET: VentaController
        public ActionResult Index()
        {
            List<Venta> lasVentas;
            lasVentas = ElAdministrador.ObtenLaListaDeVentas();
            
            return View(lasVentas);
        }

        public ActionResult Carrito(int id)
        {
            List<VentaDetalles> laListaDeDetalles = ElAdministrador.ObtengaLosItemsDeUnaVenta(id);
            var detallesConNombre = laListaDeDetalles.Select(detalle => new VentaDetalleVM
            {
                Id = detalle.Id,
                Id_Venta = detalle.Id_Venta,
                Id_Inventario = detalle.Id_Inventario,
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio,
                Monto = detalle.Monto,
                MontoDescuento = detalle.MontoDescuento,
                NombreInventario = ElAdministrador.ObtengaElInventario(detalle.Id_Inventario).Nombre
            }).ToList();

            ViewBag.IdVenta = id;
            return View(detallesConNombre);
        }


        // GET: VentaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

  
        public ActionResult AgregarVenta()
        {
            List<Inventario> laListaDeInventarios;
            laListaDeInventarios = ElAdministrador.ObtenLaListaDeInventarios();

            Venta_VentaDetalleVM elModeloAuxiliarDeVenta = new Venta_VentaDetalleVM
            {
                ItemsInventario = laListaDeInventarios
            };

            return View(elModeloAuxiliarDeVenta);
        }

        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarVenta(Venta_VentaDetalleVM laVenta)
        {
            Venta nuevaVenta = new Venta
            {

                NombreCliente = laVenta.NombreCliente,
                Fecha = DateTime.Now,
                TipoDePago = TipoDePago.PorDefinir,
                Total = 0,
                SubTotal = 0,
                PorcentajeDescuento = 0,
                MontoDescuento = 0,
                Estado = EstadoVenta.EnProceso,
                IdAperturaDeCaja = ElAdministrador.ObtenerIdCajaAbierta(User.Identity.Name)

            };

            int idNuevaVenta = ElAdministrador.AgregueVenta(nuevaVenta);

            VentaDetalles nuevoDetalle = new VentaDetalles
            {
                Cantidad = laVenta.Cantidad,
                Precio = ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                Monto = laVenta.Cantidad * ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                MontoDescuento = 0,
                Id_Inventario = laVenta.IdItemSeleccionado,
                Id_Venta = idNuevaVenta
            };

            nuevaVenta.SubTotal = nuevoDetalle.Monto;
            nuevaVenta.MontoDescuento = nuevoDetalle.MontoDescuento;
            nuevaVenta.Total = nuevaVenta.SubTotal - nuevoDetalle.MontoDescuento;

            ElAdministrador.ActualiceVenta(idNuevaVenta, nuevaVenta);

            ElAdministrador.AgregueDetalleVenta(nuevoDetalle);

            return RedirectToAction("Index");
        }

        public ActionResult AgregarDetalleVenta(int idVenta)
        {
            List<Inventario> laListaDeInventarios = ElAdministrador.ObtenLaListaDeInventarios();
            List<VentaDetalles> listaDeInventariosYaAniadidos = ElAdministrador.ObtengaLosItemsDeUnaVenta(idVenta);
            var idsInventariosYaAniadidos = listaDeInventariosYaAniadidos.Select(d => d.Id_Inventario).ToList();
            List<Inventario> laListaDeInventarioActualesDisponibles = laListaDeInventarios
                .Where(inv => !idsInventariosYaAniadidos.Contains(inv.Id))
                .ToList();
            string elNombre = ElAdministrador.ObtengaNombreDeVenta(idVenta);
            Venta_VentaDetalleVM elModeloAuxiliarDeVenta = new Venta_VentaDetalleVM
            {
                ItemsInventario = laListaDeInventarioActualesDisponibles,
                NombreCliente = elNombre,
                idVenta = idVenta
            };
            return View(elModeloAuxiliarDeVenta);
        }


        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarDetalleVenta(Venta_VentaDetalleVM laVenta)
        {
            VentaDetalles nuevoDetalle = new VentaDetalles
            {
                Cantidad = laVenta.Cantidad,
                Precio = ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                Monto = laVenta.Cantidad * ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                MontoDescuento = 0,
                Id_Inventario = laVenta.IdItemSeleccionado,
                Id_Venta = laVenta.idVenta
            };
            ElAdministrador.AgregueDetalleVenta(nuevoDetalle);

            
            ViewBag.IdVenta = laVenta.idVenta;

            ElAdministrador.ActualiceElTotalEnElIndexDeVentas(laVenta.idVenta, nuevoDetalle);

            return RedirectToAction("Carrito", new { id = laVenta.idVenta });
        }

        // GET: VentaController/SeleccioneTipoDePago/5
        public ActionResult SeleccioneTipoDePago(int id)
        {
            ViewBag.IdVenta = id;
            return View();
        }

        // POST: VentaController/SeleccioneTipoDePago/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccioneTipoDePago(int id, TipoDePago tipoDePago)
        {
            try
            {
                Venta laVenta = ElAdministrador.ObtengaVentaPorId(id);
                laVenta.TipoDePago = tipoDePago;
                laVenta.Estado = EstadoVenta.Terminada;

                ElAdministrador.ActualiceVenta(id, laVenta);

                List<VentaDetalles> detallesDeLaVenta = ElAdministrador.ObtengaLosItemsDeUnaVenta(id);
                foreach (var detalle in detallesDeLaVenta)
                {
                    ElAdministrador.ActualiceLaCantidadDeInventario(detalle.Cantidad, detalle.Id_Inventario);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: VentaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VentaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VentaController/Delete/5
        public ActionResult Delete(int id)
        {

            ElAdministrador.EliminarVenta(id);


            return RedirectToAction("Index");
        }


    }
}
