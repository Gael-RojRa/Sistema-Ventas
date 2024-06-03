using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.UI.ViewModels;

namespace Proyecto_Programado.UI.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public readonly IAdministradorDeUsuarios ElAdministrador;
        public LoginController(IAdministradorDeUsuarios administrador)
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

            int agregadoCorrecto = ElAdministrador.RegistrarUsuario(usuario.Nombre, usuario.correoElectronico, usuario.Clave);
            if (agregadoCorrecto != 0)
            {
                return RedirectToAction("InicieSesion", "Login");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario, error fatal";
            return View();


        }

        [HttpGet]
        public IActionResult InicieSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InicieSesion(UsuarioLoginVM usuario)
        {
            bool lasCrendicalesSonCorrectas;
            lasCrendicalesSonCorrectas = ElAdministrador.VerifiqueCredenciales(usuario.Nombre, usuario.Clave);

            if (lasCrendicalesSonCorrectas)
            {
                ViewData["Mensaje"] = "Inicio de sesion correcto";
                return View();
            }
            else
            {
                ViewData["Mensaje"] = "Las credenciales son incorrectas";
                return View(usuario);
            }


        }
        ///METODOS DE  LOGIN GOOGLE
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.
                AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
           
            return RedirectToAction("Index");
        }
    }
}
