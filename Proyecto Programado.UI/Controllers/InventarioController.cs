﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // GET: InventarioController
        public async Task<IActionResult> Index(int id, string nombre)
        {
            List<Inventario> laListadeInventarios;
            
            var httpClient = new HttpClient();

            try
            {

                 var respuesta = await httpClient.GetAsync("https://apicomerciovs.azurewebsites.net/ModuloCatalogoDeInventarios/ObtengaLaListaDeInventarios");
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
            var url = $"https://apicomerciovs.azurewebsites.net/ModuloCatalogoDeInventarios/ObtengaInventarioConHistorico/{id}";

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();
                try
                {
                    string elNombreDeUsuario = User.Identity.Name;

                    var json = JsonConvert.SerializeObject(inventario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var url = $"https://apicomerciovs.azurewebsites.net/ModuloCatalogoDeInventarios/AgregueElInventario?elNombreDeUsuario={Uri.EscapeDataString(elNombreDeUsuario)}";

                    var respuesta = await httpClient.PostAsync(url, content);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(inventario);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }

            return View(inventario);
        }




        // GET: InventarioController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Model.Inventario inventario;

            
            var httpClient = new HttpClient();

            var url = $"https://apicomerciovs.azurewebsites.net/ModuloCatalogoDeInventarios/ObtengaElInventario/{id}";
            var respuesta = await httpClient.GetAsync(url);
            inventario = JsonConvert.DeserializeObject<Model.Inventario>(await respuesta.Content.ReadAsStringAsync());
            return View(inventario);
        }

        // POST: InventarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Model.Inventario inventario)
        {
            var httpClient = new HttpClient();
            try
            {
                string elNombreDeUsuario = User.Identity.Name;

                var json = JsonConvert.SerializeObject(inventario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"https://apicomerciovs.azurewebsites.net/ModuloCatalogoDeInventarios/EditeElInventario/{elNombreDeUsuario}";
                var respuesta = await httpClient.PutAsync(url, content);


                int idInventario = (int)(TempData["Id_Inventario"]);

                return RedirectToAction("Index", new { id = idInventario });
            }
            catch
            {
                return View();
            }
        }

    }
}
