using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Programado.SI.Controllers
{
    public class ModuloDeAperturaYCierreDeCajasController : Controller
    {
        // GET: ModuloDeAperturaYCierreDeCajasController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ModuloDeAperturaYCierreDeCajasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModuloDeAperturaYCierreDeCajasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModuloDeAperturaYCierreDeCajasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ModuloDeAperturaYCierreDeCajasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModuloDeAperturaYCierreDeCajasController/Edit/5
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

        // GET: ModuloDeAperturaYCierreDeCajasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModuloDeAperturaYCierreDeCajasController/Delete/5
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
