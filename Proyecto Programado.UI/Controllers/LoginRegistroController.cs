using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using System.Security.Claims;
using Proyecto_Programado.UI.ViewModels;
using Microsoft.AspNetCore.Authentication.Facebook;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

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

        public async Task<IActionResult> Registrarse(UsuarioVM usuario)
        {
            if (usuario.Clave != usuario.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View(usuario);
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var checkUrl = $"https://localhost:7237/api/ModuloLoginRegistro/ObtengaElUsuarioPorNombre/{usuario.Nombre}";
                    var checkResponse = await httpClient.GetAsync(checkUrl);

                    if (checkResponse.IsSuccessStatusCode)
                    {
                        var content = await checkResponse.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            ViewData["Mensaje"] = "El nombre de usuario ya está en uso";
                            return View(usuario);
                        }
                    }

                    var url = "https://localhost:7237/api/ModuloLoginRegistro/SoliciteElRegistro";

                    var registroDto = new RegistroDTO
                    {
                        NombreUsuario = usuario.Nombre,
                        Email = usuario.correoElectronico,
                        Clave = usuario.Clave
                    };

                    var registroJson = JsonConvert.SerializeObject(registroDto);
                    var content2 = new StringContent(registroJson, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(url, content2);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("SolicitudPendiente", "LoginRegistro");
                    }
                    else
                    {
                        ViewData["Mensaje"] = "No se pudo crear el usuario, error en la API";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    ViewData["Mensaje"] = "Ocurrió un error al registrar el usuario";
                }
            }

            return View(usuario);
        }





        [HttpGet]
        public IActionResult InicieSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InicieSesionAsync(UsuarioLoginVM usuario)
        {
            try
            {
                // Obtener la lista de solicitudes pendientes
                var solicitudUrl = "https://localhost:7237/api/ModuloLoginRegistro/ObtengaLaLista";

                using (var httpClient = new HttpClient())
                {
                    var solicitudResponse = await httpClient.GetAsync(solicitudUrl);

                    if (!solicitudResponse.IsSuccessStatusCode)
                    {
                        ViewData["Mensaje"] = $"Error al obtener la lista de solicitudes: {solicitudResponse.StatusCode}";
                        return View(usuario);
                    }

                    var solicitudContent = await solicitudResponse.Content.ReadAsStringAsync();
                    var laListaDeSolicitudes = JsonConvert.DeserializeObject<List<SolicitudRegistro>>(solicitudContent);

                    // Verificar si hay una solicitud pendiente para el usuario
                    var solicitudPendiente = laListaDeSolicitudes.FirstOrDefault(item =>
                        item.Nombre == usuario.NombreUsuario && item.EstadoRegistro == EstadoRegistro.Pendiente);

                    if (solicitudPendiente != null)
                    {
                        return RedirectToAction("SolicitudPendiente", "LoginRegistro");
                    }

                    // Verificar las credenciales del usuario
                    var credencialesUrl = "https://localhost:7237/api/ModuloLoginRegistro/VerifiqueCredenciales";
                    var credencialesDto = new
                    {
                        NombreUsuario = usuario.NombreUsuario,
                        Clave = usuario.Clave
                    };

                    var json = JsonConvert.SerializeObject(credencialesDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(credencialesUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultJson = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<bool>(resultJson);

                        if (resultado)
                        {
                            // Obtener los claims del usuario
                            var claimsUrl = $"https://localhost:7237/api/ModuloLoginRegistro/ObtengaElRolDelUsuario/{usuario.NombreUsuario}";
                            var claimsResponse = await httpClient.GetAsync(claimsUrl);

                            if (!claimsResponse.IsSuccessStatusCode)
                            {
                                ViewData["Mensaje"] = $"Error al obtener los claims del usuario: {claimsResponse.StatusCode}";
                                return View(usuario);
                            }

                            var rolValor = await claimsResponse.Content.ReadAsStringAsync();
                            var rolEntero = int.Parse(rolValor);
                            var rolString = Enum.GetName(typeof(Rol), rolEntero);

                            List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                        new Claim(ClaimTypes.Role, rolString)
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
                            ViewData["Mensaje"] = "Las credenciales son incorrectas o la cuenta está bloqueada";
                            return View(usuario);
                        }
                    }
                    else
                    {
                        ViewData["Mensaje"] = "Las credenciales son incorrectas o la cuenta está bloqueada";
                        return View(usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                ViewData["Mensaje"] = "Ocurrió un error al iniciar sesión";
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

            var response = await httpClient.PutAsync("https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/CambieLaClave", jsonContent);

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
