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

            return View();
        }

        [HttpPost]
        public IActionResult Abrir()
        {
            ElAdministrador.AbrirCaja();
            return RedirectToAction("Index");
        }
    }
}

