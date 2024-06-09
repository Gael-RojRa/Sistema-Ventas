using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;

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
        public ActionResult Index(int id, string nombre)
        {
            List<Inventario> laListadeInventarios;
            laListadeInventarios = ElAdministrador.ObtenLaListaDeInventarios();

            TempData["Id_Inventario"] = id;

            if (nombre is null)
            {
                return View(laListadeInventarios);
            } else
            {
                List<Inventario> laListadeInventariosFiltrada;
                laListadeInventariosFiltrada = laListadeInventarios.Where(x => x.Nombre.Contains(nombre)).ToList();
                return View(laListadeInventariosFiltrada);
            }
            
        }

        // GET: InventarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InventarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                ElAdministrador.AgregueelInventario(inventario, User.Identity.Name);

                return RedirectToAction(nameof(Index));
            }
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
                ElAdministrador.EditeElInventario(inventario);


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
