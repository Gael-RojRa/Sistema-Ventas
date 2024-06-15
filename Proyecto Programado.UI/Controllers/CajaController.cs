using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using System.Xml.Linq;

namespace Proyecto_Programado.UI.Controllers
{
    [Authorize]
    public class CajaController : Controller
    {
        private readonly IAdministradorDeCaja ElAdministrador;

        public CajaController(IAdministradorDeCaja administrador)
        {
            ElAdministrador = administrador;
        }

        // GET: CajaController
        public ActionResult Index()
        {
            var cajaAbierta = ElAdministrador.ObtenerCajaAbierta(User.Identity.Name);
            return View(cajaAbierta);
        }



        // GET: CajaController/Create
        public ActionResult Create(AperturaDeCaja caja)
        {
            ElAdministrador.AbrirCaja(caja, User.Identity.Name);
            return RedirectToAction(nameof(Index));

        }

        // GET: CajaController
        public ActionResult CerrarCaja(int id)
        {
            ElAdministrador.CerrarCaja(id);
            return RedirectToAction(nameof(Index));

        }

    }
}

