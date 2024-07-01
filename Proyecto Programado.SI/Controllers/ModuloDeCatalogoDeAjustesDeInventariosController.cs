using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Programado.SI.Controllers
{
    public class ModuloDeCatalogoDeAjustesDeInventariosController : Controller
    {
        // GET: ModuloDeCatalogoDeAjustesDeInventariosController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ModuloDeCatalogoDeAjustesDeInventariosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModuloDeCatalogoDeAjustesDeInventariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModuloDeCatalogoDeAjustesDeInventariosController/Create
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

        // GET: ModuloDeCatalogoDeAjustesDeInventariosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModuloDeCatalogoDeAjustesDeInventariosController/Edit/5
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

        // GET: ModuloDeCatalogoDeAjustesDeInventariosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModuloDeCatalogoDeAjustesDeInventariosController/Delete/5
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
