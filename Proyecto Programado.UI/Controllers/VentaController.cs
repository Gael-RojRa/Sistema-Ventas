using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programado.BL;
using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using System.Security.Claims;


namespace Proyecto_Programado.UI.Controllers
{
    [Authorize]
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
            lasVentas = ElAdministrador.ObtengaLaListaDeVentas();
            
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
                Total = detalle.MontoFinal,
                NombreInventario = ElAdministrador.ObtengaElInventario(detalle.Id_Inventario).Nombre
            }).ToList();

            ViewBag.IdVenta = id;
            return View(detallesConNombre);
        }
        public ActionResult CarritoTerminado(int id)
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
                Total = detalle.MontoFinal,
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
            laListaDeInventarios = ElAdministrador.ObtengaLaListaDeInventarios();

            Venta_VentaDetalleVM elModeloAuxiliarDeVenta = new Venta_VentaDetalleVM
            {
                ItemsInventario = laListaDeInventarios
            };
            bool cajaAbierta = ElAdministrador.VerifiqueLaCajaAbierta(User.Identity.Name); 
            ViewBag.CajaAbierta = cajaAbierta;
            return View(elModeloAuxiliarDeVenta);
        }

        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarVenta(Venta_VentaDetalleVM laVenta, int cantidadSeleccionada)
        {
            laVenta.Cantidad = cantidadSeleccionada;

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
                IdAperturaDeCaja = ElAdministrador.ObtengaElIdDeLaCajaAbierta(User.Identity.Name)

            };

            int idNuevaVenta = ElAdministrador.AgregueLaVenta(nuevaVenta);

            VentaDetalles nuevoDetalle = new VentaDetalles
            {
                Cantidad = laVenta.Cantidad,
                Precio = ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                Monto = laVenta.Cantidad * ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                MontoDescuento = 0,
                MontoFinal = laVenta.Cantidad * ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                Id_Inventario = laVenta.IdItemSeleccionado,
                Id_Venta = idNuevaVenta
            };

            nuevaVenta.SubTotal = nuevoDetalle.Monto;
            nuevaVenta.MontoDescuento = nuevoDetalle.MontoDescuento;
            nuevaVenta.Total = nuevaVenta.SubTotal - nuevoDetalle.MontoDescuento;

            ElAdministrador.ActualiceLaVenta(idNuevaVenta, nuevaVenta);

            ElAdministrador.AgregueDetalleVenta(nuevoDetalle);

            return RedirectToAction("Index");
        }

        public ActionResult AgregarDetalleVenta(int idVenta)
        {
            List<Inventario> laListaDeInventarios = ElAdministrador.ObtengaLaListaDeInventarios();
            List<VentaDetalles> listaDeInventariosYaAniadidos = ElAdministrador.ObtengaLosItemsDeUnaVenta(idVenta);
            var idsInventariosYaAniadidos = listaDeInventariosYaAniadidos.Select(d => d.Id_Inventario).ToList();
            List<Inventario> laListaDeInventarioActualesDisponibles = laListaDeInventarios
                .Where(inv => !idsInventariosYaAniadidos.Contains(inv.Id))
                .ToList();
            string elNombre = ElAdministrador.ObtengaElNombreDeVenta(idVenta);
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
                MontoFinal = laVenta.Cantidad * ElAdministrador.ObtengaElPrecioDelInventario(laVenta.IdItemSeleccionado),
                Id_Inventario = laVenta.IdItemSeleccionado,
                Id_Venta = laVenta.idVenta
            };
            ElAdministrador.AgregueDetalleVenta(nuevoDetalle);

            
            ViewBag.IdVenta = laVenta.idVenta;

            ElAdministrador.ActualiceElTotalEnElIndexDeVentas(nuevoDetalle.Id);

            return RedirectToAction("Carrito", new { id = laVenta.idVenta });
        }


        [HttpPost]
        public IActionResult AplicarDescuento(int porcentajeDescuento, int idVenta)
        {

           ElAdministrador.ApliqueElDescuento(porcentajeDescuento, idVenta);

            return RedirectToAction("Carrito", new { id = idVenta });
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
                List<VentaDetalles> detallesDeLaVenta = ElAdministrador.ObtengaLosItemsDeUnaVenta(id);

                bool inventarioSuficiente = true; 

                foreach (var detalle in detallesDeLaVenta)
                {
                    if (detalle.Cantidad > ElAdministrador.ObtengaElInventario(detalle.Id_Inventario).Cantidad)
                    {
                        string nombreInventario = ElAdministrador.ObtengaElInventario(detalle.Id_Inventario).Nombre;
                        ViewData["Cantidad"] = "La cantidad a comprar es mayor a la cantidad disponible del inventario: " + nombreInventario + ", por favor verifique su carrito";
                        inventarioSuficiente = false;  
                        break;  
                    }
                }

                if (!inventarioSuficiente)
                {
                    
                    return View();
                }
                else
                {
                    foreach (var detalle in detallesDeLaVenta)
                    {
                        ElAdministrador.ActualiceLaCantidadDeInventario(detalle.Cantidad, detalle.Id_Inventario);
                    }

                    laVenta.TipoDePago = tipoDePago;
                    laVenta.Estado = EstadoVenta.Terminada;
                    ElAdministrador.ActualiceLaVenta(id, laVenta);
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

            ElAdministrador.ElimineLaVenta(id);


            return RedirectToAction("Index");
        }
        
    }
}