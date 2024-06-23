using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_Programado.UI.Controllers
{
    public class SolicitudDeRegistrosController : Controller
    {

        // GET: SolicitudDeRegistrosController
        public readonly IAdministradorDeSolicitudes ElAdministrador;
        public SolicitudDeRegistrosController(IAdministradorDeSolicitudes administrador)
        {
            ElAdministrador = administrador;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrarse(UsuarioVM usuario)
        {
            if (usuario.Clave != usuario.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            bool solicitudExitosa = ElAdministrador.SoliciteRegistro(usuario.Nombre, usuario.correoElectronico, usuario.Clave);

            if (solicitudExitosa != false)
            {
                ViewData["Mensaje"] = "Tu solicitud ha sido enviada. Espera la aprobación del administrador.";
                return RedirectToAction("InicieSesion", "Login");

            }

            ViewData["Mensaje"] = "No se pudo crear el usuario, error fatal";
            return View();

        }

        // GET: SolicitudDeRegistrosController
        public ActionResult Index()
        {
            List<SolicitudRegistro> laListadeSolicitudes;
            laListadeSolicitudes = ElAdministrador.ObtengaLaListaDePendientes();
            return View(laListadeSolicitudes);
        }

        public ActionResult Activar(int id)
        {
            ElAdministrador.AprobarSolicitud(id);
            return RedirectToAction(nameof(Index));
        }

       

        // GET: SolicitudDeRegistrosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SolicitudDeRegistrosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SolicitudDeRegistrosController/Create
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

        // GET: SolicitudDeRegistrosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SolicitudDeRegistrosController/Edit/5
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

        // GET: SolicitudDeRegistrosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SolicitudDeRegistrosController/Delete/5
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
