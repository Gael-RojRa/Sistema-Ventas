using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Proyecto_Programado.BL;
using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using System.Net.Http;
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
        public async Task<ActionResult> Index()
        {
            List<Venta> lasVentas = new List<Venta>();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:7237/ObtengaLaListaDeVentas");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                lasVentas = JsonConvert.DeserializeObject<List<Venta>>(jsonResponse);
            }

            return View(lasVentas);
        }

        public async Task<IActionResult> Carrito(int id)
        {
            List<VentaDetalles> laListaDeDetalles = new List<VentaDetalles>();
            List<VentaDetalleVM> detallesConNombre = new List<VentaDetalleVM>();

            var httpClient = new HttpClient();

            // Obtener los items de una venta
            var detallesResponse = await httpClient.GetAsync($"https://localhost:7237/ObtengaLosItemsDeUnaVenta/{id}");
            if (detallesResponse.IsSuccessStatusCode)
            {
                var detallesJson = await detallesResponse.Content.ReadAsStringAsync();
                laListaDeDetalles = JsonConvert.DeserializeObject<List<VentaDetalles>>(detallesJson);
            }

            foreach (var detalle in laListaDeDetalles)
            {
                // Obtener el inventario por cada detalle
                var inventarioResponse = await httpClient.GetAsync($"https://localhost:7237/ModuloCatalogoDeInventarios/ObtengaElInventario/{detalle.Id_Inventario}");
                if (inventarioResponse.IsSuccessStatusCode)
                {
                    var inventarioJson = await inventarioResponse.Content.ReadAsStringAsync();
                    var inventario = JsonConvert.DeserializeObject<Inventario>(inventarioJson);

                    detallesConNombre.Add(new VentaDetalleVM
                    {
                        Id = detalle.Id,
                        Id_Venta = detalle.Id_Venta,
                        Id_Inventario = detalle.Id_Inventario,
                        Cantidad = detalle.Cantidad,
                        Precio = detalle.Precio,
                        Monto = detalle.Monto,
                        MontoDescuento = detalle.MontoDescuento,
                        Total = detalle.MontoFinal,
                        NombreInventario = inventario.Nombre
                    });
                }
            }

            ViewBag.IdVenta = id;
            return View(detallesConNombre);

            //Forma antigua sin api
            //List<VentaDetalles> laListaDeDetalles = ElAdministrador.ObtengaLosItemsDeUnaVenta(id);
            //var detallesConNombre = laListaDeDetalles.Select(detalle => new VentaDetalleVM
            //{
            //    Id = detalle.Id,
            //    Id_Venta = detalle.Id_Venta,
            //    Id_Inventario = detalle.Id_Inventario,
            //    Cantidad = detalle.Cantidad,
            //    Precio = detalle.Precio,
            //    Monto = detalle.Monto,
            //    MontoDescuento = detalle.MontoDescuento,
            //    Total = detalle.MontoFinal,
            //    NombreInventario = ElAdministrador.ObtengaElInventario(detalle.Id_Inventario).Nombre
            //}).ToList();

            //ViewBag.IdVenta = id;
            //return View(detallesConNombre);
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

        public async Task <ActionResult> AgregarDetalleVenta(int idVenta)
        {

            List<Inventario> laListaDeInventarios = new List<Inventario>();
            List<VentaDetalles> listaDeInventariosYaAniadidos = new List<VentaDetalles>();
            string elNombre = string.Empty;

            var httpClient = new HttpClient();

            var inventariosResponse = await httpClient.GetAsync("https://localhost:7237/ObtengaLaListaDeInventarios");
            if (inventariosResponse.IsSuccessStatusCode)
            {
                var inventariosJson = await inventariosResponse.Content.ReadAsStringAsync();
                laListaDeInventarios = JsonConvert.DeserializeObject<List<Inventario>>(inventariosJson);
            }

            var detallesResponse = await httpClient.GetAsync($"https://localhost:7237/ObtengaLosItemsDeUnaVenta/{idVenta}");
            if (detallesResponse.IsSuccessStatusCode)
            {
                var detallesJson = await detallesResponse.Content.ReadAsStringAsync();
                listaDeInventariosYaAniadidos = JsonConvert.DeserializeObject<List<VentaDetalles>>(detallesJson);
            }

            var nombreResponse = await httpClient.GetAsync($"https://localhost:7237/ObtengaElNombreDeVenta/{idVenta}");
            if (nombreResponse.IsSuccessStatusCode)
            {
                elNombre = await nombreResponse.Content.ReadAsStringAsync();
            }

            var idsInventariosYaAniadidos = listaDeInventariosYaAniadidos.Select(d => d.Id_Inventario).ToList();
            List<Inventario> laListaDeInventarioActualesDisponibles = laListaDeInventarios
                .Where(inv => !idsInventariosYaAniadidos.Contains(inv.Id))
                .ToList();

            Venta_VentaDetalleVM elModeloAuxiliarDeVenta = new Venta_VentaDetalleVM
            {
                ItemsInventario = laListaDeInventarioActualesDisponibles,
                NombreCliente = elNombre,
                idVenta = idVenta
            };
            return View(elModeloAuxiliarDeVenta);
            //Implementacion anterior sin api
            //List<Inventario> laListaDeInventarios = ElAdministrador.ObtengaLaListaDeInventarios();
            //List<VentaDetalles> listaDeInventariosYaAniadidos = ElAdministrador.ObtengaLosItemsDeUnaVenta(idVenta);
            //var idsInventariosYaAniadidos = listaDeInventariosYaAniadidos.Select(d => d.Id_Inventario).ToList();
            //List<Inventario> laListaDeInventarioActualesDisponibles = laListaDeInventarios
            //    .Where(inv => !idsInventariosYaAniadidos.Contains(inv.Id))
            //    .ToList();
            //string elNombre = ElAdministrador.ObtengaElNombreDeVenta(idVenta);
            //Venta_VentaDetalleVM elModeloAuxiliarDeVenta = new Venta_VentaDetalleVM
            //{
            //    ItemsInventario = laListaDeInventarioActualesDisponibles,
            //    NombreCliente = elNombre,
            //    idVenta = idVenta
            //};
            //return View(elModeloAuxiliarDeVenta);
        }

        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarDetalleVenta(Venta_VentaDetalleVM laVenta, int cantidadSeleccionada)
        {
            laVenta.Cantidad = cantidadSeleccionada;
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
        public async Task<IActionResult> AplicarDescuento(int porcentajeDescuento, int idVenta)
        {

            var httpClient = new HttpClient();
            var requestUri = $"https://localhost:7237/apliqueDescuento/{idVenta}/{porcentajeDescuento}";

            var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Carrito", new { id = idVenta });
            }
            else
            {
                // Manejar el error adecuadamente
                return View("Error");
            }
            //Forma Anterior, sin api
            //ElAdministrador.ApliqueElDescuento(porcentajeDescuento, idVenta);
            //return RedirectToAction("Carrito", new { id = idVenta });
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
        public async Task<ActionResult> Delete(int id, int idVenta)
        {
            var httpClient = new HttpClient();
            var requestUri = $"https://localhost:7237/ElimineLaVenta/{id}";

            var response = await httpClient.DeleteAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {

                return View("Error");
            }
            //funcionamiento antiguo sin API.
            //ElAdministrador.ElimineLaVenta(id);
            //return RedirectToAction("Index");
        }

    }
}