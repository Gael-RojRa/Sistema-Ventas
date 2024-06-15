using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

            bool agregadoCorrecto = ElAdministrador.RegistreElUsuario(usuario.Nombre, usuario.correoElectronico, usuario.Clave);
            if (agregadoCorrecto != false)
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
        public async Task<IActionResult> InicieSesionAsync(UsuarioLoginVM usuario)
        {
            bool lasCrendicalesSonCorrectas;
            lasCrendicalesSonCorrectas = ElAdministrador.VerifiqueCredenciales(usuario.NombreUsuario, usuario.Clave);

            if (lasCrendicalesSonCorrectas)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Role, ElAdministrador.ObtengaElRolDelUsuario(usuario.NombreUsuario).ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                    );


                
                    return RedirectToAction("Index", "Inventario");

                

            }
            else
            {
                ViewData["Mensaje"] = "Las credenciales son incorrectas o la cuenta esta bloqueada";
                return View(usuario);
            }


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

            if (nameClaim == null)
            {
                return BadRequest("Claim de Name no encontrado");
            }

            var usuarioName = nameClaim.Value;

            return RedirectToAction("Index", "Inventario");
        }


        public async Task<IActionResult> LoginWithFacebook()
        {
            try
            {
                var redirectUrl = Url.Action("FacebookResponse", "Login");
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
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (!result.Succeeded)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }

                var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

                return RedirectToAction("Index", "Inventario");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        // GET: LoginController/CambiarClave
        [HttpGet]
        public IActionResult CambiarClave()
        {
            return View();
        }

        // POST: LoginController/CambiarClave
        [HttpPost]
        public IActionResult CambiarClave(CambioClaveVM modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = ElAdministrador.ObtengaElUsuarioPorNombre(modelo.Nombre);
            if (usuario == null)
            {
                ViewData["Mensaje"] = "Usuario no encontrado.";
                return View(modelo);
            }

            ElAdministrador.CambieLaClave(usuario, modelo.ClaveNueva);

            string asunto = "Cambio de clave";
            string contenido = $"Le informamos que el cambio de clave de la cuenta del usuario {modelo.Nombre} se ejecutó satisfactoriamente.";
            ElAdministrador.EnvieElCorreoElectronico(usuario.correoElectronico, asunto, contenido);

            ViewData["Mensaje"] = "Cambio de clave realizado correctamente.";
            return View(modelo);
        }

        [HttpGet]
        public IActionResult DeBienvenida(string usuario)
        {
            TempData["Usuario"] = usuario;
            return View();
        }

    }
}