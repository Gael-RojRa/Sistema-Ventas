using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using Newtonsoft.Json;
using Proyecto_Programado.UI.Models;
using System.Diagnostics;
using System.Text;

namespace Proyecto_Programado.UI.Controllers
{

    [Authorize]
    public class InventarioController : Controller
    {

        public readonly IAdministradorDeInventarios ElAdministrador;

        public InventarioController(IAdministradorDeInventarios administrador)
        {
            ElAdministrador = administrador; 
        }
        // GET: InventarioController
        public async Task<IActionResult> Index(int id, string nombre)
        {
            List<Inventario> laListadeInventarios;
            
            var httpClient = new HttpClient();

            try
            {

                var respuesta = await httpClient.GetAsync("https://localhost:7237/ModuloCatalogoDeInventarios/ObtengaLaListaDeInventarios");
                string apiResponse = await respuesta.Content.ReadAsStringAsync();
                laListadeInventarios = JsonConvert.DeserializeObject<List<Model.Inventario>>(apiResponse);

                TempData["Id_Inventario"] = id;

                if (nombre is null)
                    return View(laListadeInventarios);
                else
                {
                    List<Inventario> laListadeInventariosFiltrada;
                    laListadeInventariosFiltrada = laListadeInventarios.Where(x => x.Nombre.Contains(nombre)).ToList();
                    return View(laListadeInventariosFiltrada);
                }
            }

            catch (Exception ex)
            {
                return View();
            }

            


        }

        // GET: InventarioController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var httpClient = new HttpClient();
            var url = $"https://localhost:7237/ModuloCatalogoDeInventarios/ObtengaInventarioConHistorico/{id}";

            try
            {
                var respuesta = await httpClient.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var apiResponse = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);

                    // Convertir el JSON a objetos definidos
                    var inventario = JsonConvert.DeserializeObject<Inventario>(resultado["inventario"].ToString());
                    var historicoInventarios = JsonConvert.DeserializeObject<List<HistoriconInventario>>(resultado["historicoInventarios"].ToString());

                    ViewBag.HistoricoInventarios = historicoInventarios;
                    return View(inventario);
                }
                else
                {
                    var errorMessage = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {respuesta.StatusCode}, Message: {errorMessage}");
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }






        // GET: InventarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();
                try
                {
                    // Obtener el nombre de usuario actual
                    var elNombreDeUsuario = User.Identity.Name;

                    // Crear un objeto anónimo que incluya el inventario y el nombre de usuario
                    var objetoAEnviar = new
                    {
                        elInventario = inventario,
                        elNombreDeUsuario = elNombreDeUsuario
                    };

                    // Serializar el objeto anónimo a JSON
                    var json = JsonConvert.SerializeObject(objetoAEnviar);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Enviar la solicitud POST
                    var respuesta = await httpClient.PostAsync("https://localhost:7237/ModuloCatalogoDeInventarios/AgregueElInventario", content);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Manejar el caso en que la respuesta no es exitosa
                        return View(inventario);
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción
                    Console.WriteLine($"Exception: {ex.Message}");
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }

            // Si el modelo no es válido, retornar la vista con el inventario
            return View(inventario);
        }



        // GET: InventarioController/Edit/5
        public ActionResult Edit(int id)
        {
            Model.Inventario inventario;
            inventario = ElAdministrador.ObtengaElInventario(id);

            return View(inventario);
        }

        // POST: InventarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model.Inventario inventario)
        {
            try
            {
                ElAdministrador.EditeElInventario(inventario, User.Identity.Name);


                int idInventario = (int)(TempData["Id_Inventario"]);

                return RedirectToAction("Index", new { id = idInventario });
            }
            catch
            {
                return View();
            }
        }

        // GET: InventarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InventarioController/Delete/5
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

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Opcionalmente, elimina las cookies manualmente si es necesario
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete(".AspNetCore.Google");
            Response.Cookies.Delete(".AspNetCore.Facebook");

            return RedirectToAction("InicieSesion", "Login");
        }


    }
}
