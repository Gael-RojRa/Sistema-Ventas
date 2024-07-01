using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Programado.SI.Controllers
{
    public class ModuloDeVentasController : Controller
    {
        // GET: ModuloDeVentasController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ModuloDeVentasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModuloDeVentasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModuloDeVentasController/Create
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

        // GET: ModuloDeVentasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModuloDeVentasController/Edit/5
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

        // GET: ModuloDeVentasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModuloDeVentasController/Delete/5
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
