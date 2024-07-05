using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Proyecto_Programado.UI.ViewModels;

namespace Proyecto_Programado.UI.Controllers
{
    [Authorize]
    public class AjusteController : Controller
    {
        public readonly IAdministradorDeAjustes ElAdministrador;

        public AjusteController(IAdministradorDeAjustes administrador)
        {
            ElAdministrador = administrador;
        }

        // GET: AjusteController
        public async Task<IActionResult> Index(int id, string nombre)
        {
            List<Inventario> laListaDeInventarios;
            var httpClient = new HttpClient();
            try
            {

                var respuesta = await httpClient.GetAsync("https://localhost:7237/api/ModuloDeAjustes/ObtengaLaListaDeInventarios");

                string apiResponse = await respuesta.Content.ReadAsStringAsync();
                laListaDeInventarios = JsonConvert.DeserializeObject<List<Model.Inventario>>(apiResponse);

                TempData["Id_Inventario"] = id;

                if (nombre is null)
                    return View(laListaDeInventarios);
                else
                {
                    List<Inventario> laListadeInventariosFiltrada;
                    laListadeInventariosFiltrada = laListaDeInventarios.Where(x => x.Nombre.Contains(nombre)).ToList();
                    return View(laListadeInventariosFiltrada);
                }
            }

            catch (Exception ex)
            {
                return View();
            }



        }



        // GET: AjusteController
        public async Task<ActionResult> MostrarAjustes(int id)
        {
            List<AjusteDeInventario> laListaDeAjustes = new List<AjusteDeInventario>();
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"https://localhost:7237/api/ModuloDeAjustes/ObtengaLosAjustes?id={id}");


            var json = await response.Content.ReadAsStringAsync();
            laListaDeAjustes = JsonConvert.DeserializeObject<List<AjusteDeInventario>>(json);


            return View(laListaDeAjustes);
        }



        // GET: AjusteController/Details/5     
        public async Task<IActionResult> Details(int id)
        {
            Model.AjusteDeInventario ajustesInventario;
            var httpClient = new HttpClient();
            try
            {
                var query = new Dictionary<string, string>()
                {
                    ["id"] = id.ToString()
                };
                var uri = QueryHelpers.AddQueryString("https://localhost:7237/api/ModuloDeAjustes/ObtengaLosAjustesDeInventario", query);
                var respuesta = await httpClient.GetAsync(uri);
                string apiResponse = await respuesta.Content.ReadAsStringAsync();
                ajustesInventario = JsonConvert.DeserializeObject<Model.AjusteDeInventario>(apiResponse);
                return View(ajustesInventario);
            }

            catch (Exception ex)
            {
                return View();
            }
        }



        // GET: AjusteController/Create
        public async Task<ActionResult> AgregarAjuste(int id_inventario)
        {
            var httpClient = new HttpClient();

            try
            {
                var query = new Dictionary<string, string>()
                {

                    ["id"] = id_inventario.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://localhost:7237/api/ModuloDeAjustes/ObtengaLaCantidadActual", query);

                var response = await httpClient.GetAsync(uri);

                

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var cantidad = JsonConvert.DeserializeObject<int>(apiResponse);

                    var cantidadActual = new Model.AjusteDeInventario
                    {
                        CantidadActual = cantidad,
                        Id_Inventario = id_inventario,
                        Ajuste = 0,
                        UserId = User.Identity.Name
                    };

                    return View(cantidadActual);
                }
                else
                {

                    return View("Error");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error en AgregarAjuste(GET): {ex.Message}");
                return View("Error");
            }
        }

        // POST: AjusteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarAjuste(AjusteDeInventario nuevoAjuste)
        {
            var httpClient = new HttpClient();

            try
            {
                
                var json = JsonConvert.SerializeObject(nuevoAjuste);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"https://localhost:7237/api/ModuloDeAjustes/AgregueUnAjuste?elNombreDeUsuario={Uri.EscapeDataString(User.Identity.Name)}";

                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Manejar caso donde la solicitud HTTP no fue exitosa
                    // Por ejemplo, redirigir a una vista de error o mostrar un mensaje al usuario
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales aquí
                Console.WriteLine($"Error en AgregarAjuste(POST): {ex.Message}");
                return View("Error");
            }
        }



        // GET: AjusteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AjusteController/Edit/5
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

        // GET: AjusteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AjusteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}