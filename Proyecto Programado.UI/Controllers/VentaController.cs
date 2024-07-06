using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using Proyecto_Programado.BL;
using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;


namespace Proyecto_Programado.UI.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {

        // GET: VentaController
        public async Task<ActionResult> Index()
        {
            List<Venta> lasVentas = new List<Venta>();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaLaListaDeVentas");

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

            var httpClient = new HttpClient();

            // Obtener los items de una venta
            var detallesResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaLosItemsDeUnaVenta/{id}");

                var detallesJson = await detallesResponse.Content.ReadAsStringAsync();
                laListaDeDetalles = JsonConvert.DeserializeObject<List<VentaDetalles>>(detallesJson);
            

            var inventarioTasks = laListaDeDetalles.Select(async detalle =>
            {
                var inventarioResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaElInventario/{detalle.Id_Inventario}");
                inventarioResponse.EnsureSuccessStatusCode();
                var inventarioJson = await inventarioResponse.Content.ReadAsStringAsync();
                var inventario = JsonConvert.DeserializeObject<Inventario>(inventarioJson);

                return new { detalle, inventario.Nombre };
            });

            var inventarioResults = await Task.WhenAll(inventarioTasks);

            var detallesConNombre = inventarioResults.Select(result => new VentaDetalleVM
            {
                Id = result.detalle.Id,
                Id_Venta = result.detalle.Id_Venta,
                Id_Inventario = result.detalle.Id_Inventario,
                Cantidad = result.detalle.Cantidad,
                Precio = result.detalle.Precio,
                Monto = result.detalle.Monto,
                MontoDescuento = result.detalle.MontoDescuento,
                Total = result.detalle.MontoFinal,
                NombreInventario = result.Nombre
            }).ToList();

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
        public async Task<IActionResult> CarritoTerminado(int id)
        {
            var httpClient = new HttpClient();

            try 
            { 

            List<VentaDetalles> laListaDeDetalles = new List<VentaDetalles>();

            var vdetalleResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaLosItemsDeUnaVenta/{id}");

                var vdetalleJson = await vdetalleResponse.Content.ReadAsStringAsync();
                    laListaDeDetalles = JsonConvert.DeserializeObject<List<VentaDetalles>>(vdetalleJson);
            

                var inventarioTasks = laListaDeDetalles.Select(async detalle =>
                {
                    var inventarioResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaElInventario/{detalle.Id_Inventario}");
                    inventarioResponse.EnsureSuccessStatusCode();
                    var inventarioJson = await inventarioResponse.Content.ReadAsStringAsync();
                    var inventario = JsonConvert.DeserializeObject<Inventario>(inventarioJson);

                    return new { detalle, inventario.Nombre };
                });

                var inventarioResults = await Task.WhenAll(inventarioTasks);
                var detallesConNombre = inventarioResults.Select(result => new VentaDetalleVM
                {
                    Id = result.detalle.Id,
                    Id_Venta = result.detalle.Id_Venta,
                    Id_Inventario = result.detalle.Id_Inventario,
                    Cantidad = result.detalle.Cantidad,
                    Precio = result.detalle.Precio,
                    Monto = result.detalle.Monto,
                    MontoDescuento = result.detalle.MontoDescuento,
                    Total = result.detalle.MontoFinal,
                    NombreInventario = result.Nombre
                }).ToList();

                ViewBag.IdVenta = id;
            return View(detallesConNombre);

            }

            catch
            {
                return View();
            }
        }


        // GET: VentaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public async Task<ActionResult> AgregarVenta()
        {
            var httpClient = new HttpClient();


                List<Inventario> laListaDeInventarios = new List<Inventario>();

                var inventariosResponse = await httpClient.GetAsync("https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaLaListaDeInventarios");
                if (inventariosResponse.IsSuccessStatusCode)
                {
                    var inventariosJson = await inventariosResponse.Content.ReadAsStringAsync();
                    laListaDeInventarios = JsonConvert.DeserializeObject<List<Inventario>>(inventariosJson);
                }

                Venta_VentaDetalleVM elModeloAuxiliarDeVenta = new Venta_VentaDetalleVM
                {
                    ItemsInventario = laListaDeInventarios
                };

                string elNombreUsuario = User.Identity.Name;

                var verifiqueResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/VerifiqueLaCajaAbierta/{elNombreUsuario}");
                var verifiqueJson = await verifiqueResponse.Content.ReadAsStringAsync();
                bool verificacionDeCaja = JsonConvert.DeserializeObject<bool>(verifiqueJson);
                

                bool cajaAbierta = verificacionDeCaja;

                ViewBag.CajaAbierta = cajaAbierta;
                return View(elModeloAuxiliarDeVenta);

            



        }

        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>  AgregarVenta(Venta_VentaDetalleVM laVenta, int cantidadSeleccionada)
        {
            var httpClient = new HttpClient();

            laVenta.Cantidad = cantidadSeleccionada;


            var userRespone = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaElIdDeLaCajaAbierta/{User.Identity.Name}");
            var userJson = await userRespone.Content.ReadAsStringAsync();
            int idUsuario = JsonConvert.DeserializeObject<int>(userJson);


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
                IdAperturaDeCaja = idUsuario

            };

            string json = JsonConvert.SerializeObject(nuevaVenta);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync("https://apicomerciovs.azurewebsites.net/ModuloDeVentas/AgregueLaVenta", byteContent);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int idNuevaVenta = JsonConvert.DeserializeObject<int>(responseString);


            var cantidadResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaElPrecioDelInventario/{laVenta.IdItemSeleccionado}");
            cantidadResponse.EnsureSuccessStatusCode();
            var cantidadJson = await cantidadResponse.Content.ReadAsStringAsync();
            decimal cantidad = JsonConvert.DeserializeObject<decimal>(cantidadJson);


            VentaDetalles nuevoDetalle = new VentaDetalles
            {
                Cantidad = laVenta.Cantidad,
                Precio = cantidad,
                Monto = laVenta.Cantidad * cantidad,
                MontoDescuento = 0,
                MontoFinal = laVenta.Cantidad * cantidad,
                Id_Inventario = laVenta.IdItemSeleccionado,
                Id_Venta = idNuevaVenta
            };

            nuevaVenta.SubTotal = nuevoDetalle.Monto;
            nuevaVenta.MontoDescuento = nuevoDetalle.MontoDescuento;
            nuevaVenta.Total = nuevaVenta.SubTotal - nuevoDetalle.MontoDescuento;

            var actualiceVentaUrl = $"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ActualiceLaVenta/{idNuevaVenta}";

            var actualiceVentaJson = JsonConvert.SerializeObject(nuevaVenta);
            var actualiceVentaBuffer = System.Text.Encoding.UTF8.GetBytes(actualiceVentaJson);
            var actualiceVentaByteContent = new ByteArrayContent(actualiceVentaBuffer);

            actualiceVentaByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var actualiceVentarequest = new HttpRequestMessage(HttpMethod.Put, actualiceVentaUrl)
            {
                Content = actualiceVentaByteContent
            };

            var actualiceVentaResponse = await httpClient.SendAsync(actualiceVentarequest);


            string detaleJson = JsonConvert.SerializeObject(nuevoDetalle);
            var detalleBuffer = System.Text.Encoding.UTF8.GetBytes(detaleJson);
            var detalleByteContent = new ByteArrayContent(detalleBuffer);
            detalleByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await httpClient.PostAsync("https://apicomerciovs.azurewebsites.net/ModuloDeVentas/AgregueDetalleVenta", detalleByteContent);

            return RedirectToAction("Index");
        }

        public async Task <ActionResult> AgregarDetalleVenta(int idVenta)
        {

            List<Inventario> laListaDeInventarios = new List<Inventario>();
            List<VentaDetalles> listaDeInventariosYaAniadidos = new List<VentaDetalles>();
            string elNombre = string.Empty;

            var httpClient = new HttpClient();

            var inventariosResponse = await httpClient.GetAsync("https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaLaListaDeInventarios");
            if (inventariosResponse.IsSuccessStatusCode)
            {
                var inventariosJson = await inventariosResponse.Content.ReadAsStringAsync();
                laListaDeInventarios = JsonConvert.DeserializeObject<List<Inventario>>(inventariosJson);
            }

            var detallesResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaLosItemsDeUnaVenta/{idVenta}");
            if (detallesResponse.IsSuccessStatusCode)
            {
                var detallesJson = await detallesResponse.Content.ReadAsStringAsync();
                listaDeInventariosYaAniadidos = JsonConvert.DeserializeObject<List<VentaDetalles>>(detallesJson);
            }

            var nombreResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaElNombreDeVenta/{idVenta}");
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
        public async Task<ActionResult> AgregarDetalleVenta(Venta_VentaDetalleVM laVenta, int cantidadSeleccionada)
        {
            var httpClient = new HttpClient();

            laVenta.Cantidad = cantidadSeleccionada;

            var cantidadResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaElPrecioDelInventario/{laVenta.IdItemSeleccionado}");
            cantidadResponse.EnsureSuccessStatusCode();
            var cantidadJson = await cantidadResponse.Content.ReadAsStringAsync();
            decimal cantidad = JsonConvert.DeserializeObject<decimal>(cantidadJson);
            


            VentaDetalles nuevoDetalle = new VentaDetalles
            {
                Cantidad = laVenta.Cantidad,
                Precio = cantidad,
                Monto = laVenta.Cantidad * cantidad,
                MontoDescuento = 0,
                MontoFinal = laVenta.Cantidad * cantidad,
                Id_Inventario = laVenta.IdItemSeleccionado,
                Id_Venta = laVenta.idVenta
            };

            string detaleJson = JsonConvert.SerializeObject(nuevoDetalle);
            var detalleBuffer = System.Text.Encoding.UTF8.GetBytes(detaleJson);
            var detalleByteContent = new ByteArrayContent(detalleBuffer);
            detalleByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync("https://apicomerciovs.azurewebsites.net/ModuloDeVentas/AgregueDetalleVenta", detalleByteContent);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int idNuevoDetalle = JsonConvert.DeserializeObject<int>(responseString);


            ViewBag.IdVenta = laVenta.idVenta;
            

            var requestUri = $"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ActualiceElTotalEnElIndexDeVentas/{idNuevoDetalle}";

            var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

            var actualiceElTotaldelIndex = await httpClient.SendAsync(request);



            return RedirectToAction("Carrito", new { id = laVenta.idVenta });
        }


        [HttpPost]
        public async Task<IActionResult> AplicarDescuento(int porcentajeDescuento, int idVenta)
        {

            var httpClient = new HttpClient();
            var requestUri = $"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ApliqueElDescuento/{porcentajeDescuento}/{idVenta}";

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
        public async Task<ActionResult> SeleccioneTipoDePago(int id, TipoDePago tipoDePago)
        {
            var httpClient = new HttpClient();

            try
            {

                Venta laVenta = new Venta();
                List<VentaDetalles> detallesDeLaVenta = new List<VentaDetalles>();

                var ventaResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaVentaPorId/{id}");
                if (ventaResponse.IsSuccessStatusCode)
                {
                    var ventaJson = await ventaResponse.Content.ReadAsStringAsync();
                    laVenta = JsonConvert.DeserializeObject<Venta>(ventaJson);
                } 
                

                var detallesDeLaVentaResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaLaListaDeInventarios/{id}");
                if (detallesDeLaVentaResponse.IsSuccessStatusCode)
                {
                    var detallesDeLaVentaJson = await detallesDeLaVentaResponse.Content.ReadAsStringAsync();
                    detallesDeLaVenta = JsonConvert.DeserializeObject<List<VentaDetalles>>(detallesDeLaVentaJson);
                }

                bool inventarioSuficiente = true;

                foreach (var detalle in detallesDeLaVenta)
                {
                    var inventarioResponse = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ObtengaElInventario/{detalle.Id_Inventario}");
                        var inventarioJson = await inventarioResponse.Content.ReadAsStringAsync();
                        var inventario = JsonConvert.DeserializeObject<Inventario>(inventarioJson);
                    


                    if (detalle.Cantidad > inventario.Cantidad)
                    {
                        string nombreInventario = inventario.Nombre;
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

                        var inventarioResponse = $"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ActualiceLaCantidadDeInventario/{detalle.Id_Inventario}/{detalle.Cantidad}";

                        var response = await httpClient.PutAsync(inventarioResponse, null);

                    }

                    laVenta.TipoDePago = tipoDePago;
                    laVenta.Estado = EstadoVenta.Terminada;


                    var ventaUrl = $"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ActualiceLaVenta/{id}/{laVenta}";

                    var finalResponse = await httpClient.PutAsync(ventaUrl, null);
                }

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
            var requestUri = $"https://apicomerciovs.azurewebsites.net/ModuloDeVentas/ElimineLaVenta/{id}";

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