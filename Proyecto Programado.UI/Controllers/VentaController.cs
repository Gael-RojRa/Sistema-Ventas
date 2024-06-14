using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;


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
            return View();
        }

        // GET: VentaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VentaController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Venta venta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ElAdministrador.RegistreVenta(venta);
                    return RedirectToAction("Index"); // Redirecciona a la acción deseada después de registrar la venta
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(venta);
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
            return View();
        }

        // POST: VentaController/Delete/5
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
