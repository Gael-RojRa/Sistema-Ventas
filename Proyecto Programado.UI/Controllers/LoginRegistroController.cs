using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using System.Security.Claims;
using Proyecto_Programado.UI.ViewModels;
using Microsoft.AspNetCore.Authentication.Facebook;
using Newtonsoft.Json;
using System.Text;

namespace Proyecto_Programado.UI.Controllers
{
    public class LoginRegistroController : Controller
    {

        // GET: LoginRegistroController
        public readonly IAdministradorDeSolicitudes ElAdministrador;
        public readonly IAdministradorDeUsuarios    ElAdministradorDeUsuarios;
        public LoginRegistroController(IAdministradorDeSolicitudes administrador, IAdministradorDeUsuarios elAdministradorDeUsuarios)
        {
            ElAdministrador = administrador;
            ElAdministradorDeUsuarios = elAdministradorDeUsuarios;
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
                bool solicitudExitosa = ElAdministrador.SoliciteElRegistro(usuario.Nombre, usuario.correoElectronico, usuario.Clave);

                if (solicitudExitosa != false)
                {

                    return RedirectToAction("SolicitudPendiente", "LoginRegistro");

                }
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
            List<SolicitudRegistro> laListaDeSolicitudes = ElAdministrador.ObtengaLaLista();
            foreach (var item in laListaDeSolicitudes)
            {

                if (item.Nombre == usuario.NombreUsuario && item.EstadoRegistro == EstadoRegistro.Pendiente)
                {

                    return RedirectToAction("SolicitudPendiente", "LoginRegistro");

                }

            }
            bool lasCrendicalesSonCorrectas;
            lasCrendicalesSonCorrectas = ElAdministradorDeUsuarios.VerifiqueCredenciales(usuario.NombreUsuario, usuario.Clave);

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
                return RedirectToAction("SolicitudPendiente", "LoginRegistro");

            }
            else
            {
                if (usuarioExistente.EstadoRegistro == EstadoRegistro.Pendiente)
                {
                    return RedirectToAction("SolicitudPendiente", "LoginRegistro");
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
                var redirectUrl = Url.Action("FacebookResponse", "LoginRegistro");
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
                return RedirectToAction("SolicitudPendiente", "LoginRegistro");
            }
            else
            {
                if (usuarioExistente.EstadoRegistro == EstadoRegistro.Pendiente)
                {
                    return RedirectToAction("SolicitudPendiente", "LoginRegistro");
                }
                else
                {
                    return RedirectToAction("Index", "Inventario");
                }
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
        public async Task<IActionResult> CambiarClaveAsync(CambioClaveVM modelo)
        {
            var httpClient = new HttpClient();

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

            // Crear el objeto de usuario con la nueva clave
            var cambioClaveRequest = new
            {
                elUsuario = new { Nombre = modelo.Nombre },
                laClaveNueva = modelo.ClaveNueva
            };


            var jsonContent = new StringContent(JsonConvert.SerializeObject(cambioClaveRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("https://localhost:7237/api/ModuloLoginRegistro/CambieLaClave", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                string asunto = "Cambio de clave";
                string contenido = $"Le informamos que el cambio de clave de la cuenta del usuario {modelo.Nombre} se ejecutó satisfactoriamente.";
                ElAdministrador.EnvieElCorreoElectronico(usuario.correoElectronico, asunto, contenido);

                ViewData["Mensaje"] = "Cambio de clave realizado correctamente.";
            }
            else
            {
                ViewData["Mensaje"] = $"Error al cambiar la clave: {response.ReasonPhrase}";
            }

            return View(modelo);
            //FORMA ANTIGUA SIN API
            //if (!ModelState.IsValid)
            //{
            //    return View(modelo);
            //}

            //var usuario = ElAdministrador.ObtengaElUsuarioPorNombre(modelo.Nombre);
            //if (usuario == null)
            //{
            //    ViewData["Mensaje"] = "Usuario no encontrado.";
            //    return View(modelo);
            //}

            //ElAdministrador.CambieLaClave(usuario, modelo.ClaveNueva);

            //string asunto = "Cambio de clave";
            //string contenido = $"Le informamos que el cambio de clave de la cuenta del usuario {modelo.Nombre} se ejecutó satisfactoriamente.";
            //ElAdministrador.EnvieElCorreoElectronico(usuario.correoElectronico, asunto, contenido);

            //ViewData["Mensaje"] = "Cambio de clave realizado correctamente.";
            //return View(modelo);
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


        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Opcionalmente, elimina las cookies manualmente si es necesario
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete(".AspNetCore.Google");
            Response.Cookies.Delete(".AspNetCore.Facebook");

            return RedirectToAction("InicieSesion", "LoginRegistro");
        }
    }
}
