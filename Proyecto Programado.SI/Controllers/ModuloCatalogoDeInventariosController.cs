using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Programado.SI.Controllers
{
    public class ModuloCatalogoDeInventariosController : Controller
    {
        // GET: ModuloCatalogoDeInventariosController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ModuloCatalogoDeInventariosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModuloCatalogoDeInventariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModuloCatalogoDeInventariosController/Create
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

        // GET: ModuloCatalogoDeInventariosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModuloCatalogoDeInventariosController/Edit/5
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

        // GET: ModuloCatalogoDeInventariosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModuloCatalogoDeInventariosController/Delete/5
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
