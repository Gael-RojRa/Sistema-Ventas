using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using System.Xml.Linq;

namespace Proyecto_Programado.UI.Controllers
{
    public class CajaController : Controller
    {
        private readonly IAdministradorDeCaja ElAdministrador;

        public CajaController(IAdministradorDeCaja administrador)
        {
            ElAdministrador = administrador;
        }

        public IActionResult Index()
        {
            
           /* var apertura = ElAdministrador.ObtenerApertura();
            if (apertura != null)
            {
                ViewBag.FechaInicio = apertura.FechaDeInicio;
            }
                    */
            return View();
        }

        [HttpPost]
        public IActionResult Abrir()
        {
            ElAdministrador.AbrirCaja(User.Identity.Name);
            return RedirectToAction("Index");
        }

        /* [HttpPost]
        public IActionResult Cerrar()
        {
            ElAdministrador.CerrarCaja();
            return RedirectToAction("Index");
        } */
    }
}

