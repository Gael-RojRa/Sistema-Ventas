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
using Microsoft.AspNetCore.Authentication.Facebook;

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

        public async Task<IActionResult> LoginWithFacebook()
        {
            try
            {
                var redirectUrl = Url.Action("FacebookResponse", "SolicitudDeRegistros");
                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                return Challenge(properties, FacebookDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var resultado = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!resultado.Succeeded)
            {
                return Unauthorized();
            }

            var claimsPrincipal = resultado.Principal;

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

       

        
    }
}
