using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authentication.Google;

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
            var usuarioExistente = ElAdministrador.ObtengaElUsuarioPorNombre(usuario.Nombre);
            if (usuarioExistente != null)
            {
                ViewData["Mensaje"] = "El nombre de usuario ya está en uso";
                return View();
            }
            else
            {
                bool solicitudExitosa = ElAdministrador.SoliciteRegistro(usuario.Nombre, usuario.correoElectronico, usuario.Clave);

                if (solicitudExitosa != false)
                {

                    return RedirectToAction("SolicitudPendiente", "SolicitudDeRegistros");

                }
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario, error fatal";
            return View();

        }
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {

                RedirectUri = Url.Action("GoogleResponse"),

            });
        }
        public async Task<IActionResult> GoogleResponse()
        {

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var claimsPrincipal = result.Principal;

            var nameClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);
            var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            if (nameClaim == null)
            {
                return BadRequest("Claim de Name no encontrado");
            }


            var usuarioName = nameClaim.Value;
            var usuarioEmail = emailClaim;
            var usuarioExistente = ElAdministrador.ObtengaElUsuarioPorNombre(usuarioName);

            if (usuarioExistente == null)
            {
                ElAdministrador.CrearUsuario(usuarioName, usuarioEmail);
                return RedirectToAction("SolicitudPendiente", "SolicitudDeRegistros");

            }
            else
            {
                if (usuarioExistente.EstadoRegistro == EstadoRegistro.Pendiente)
                {
                    return RedirectToAction("SolicitudPendiente", "SolicitudDeRegistros");
                }
                else
                {
                    return RedirectToAction("Index", "Inventario");
                }
            }
            
           

        }

        public IActionResult SolicitudPendiente()
        {
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
