using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.Model;
using System.Security.Claims;
using Proyecto_Programado.UI.ViewModels;
using Microsoft.AspNetCore.Authentication.Facebook;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;

namespace Proyecto_Programado.UI.Controllers
{
    public class LoginRegistroController : Controller
    {

        // GET: LoginRegistroController

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
                    var checkUrl = $"https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/ObtengaElUsuarioPorNombre/{usuario.Nombre}";
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

                    var url = "https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/SoliciteElRegistro";

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
                var solicitudUrl = "https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/ObtengaLaLista";

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
                    var credencialesUrl = "https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/VerifiqueCredenciales";
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
                            var claimsUrl = $"https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/ObtengaElRolDelUsuario/{usuario.NombreUsuario}";
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

            var httpClient = new HttpClient();
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

            var nombre = nameClaim.Value;
            var usuarioEmail = emailClaim;

            var response = await httpClient.GetAsync($"https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/ObtengaElUsuarioPorNombre/{nombre}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error al obtener el usuario");
            }

            var usuarioExistente = await response.Content.ReadFromJsonAsync<SolicitudRegistro>();

            if (usuarioExistente == null)
            {

                var nuevoUsuario = new { usuario = nombre, correoElectronico = usuarioEmail };
                var createResponse = await httpClient.PostAsJsonAsync("https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/CrearUsuario", nuevoUsuario);

                if (!createResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)createResponse.StatusCode, "Error al crear el usuario");
                }

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

        /*public async Task<IActionResult> FacebookResponse()
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
        */

        // GET: LoginController/CambiarClave
        [HttpGet]
        public IActionResult CambiarClave()
        {
            return View();
        }

        // POST: LoginController/CambiarClave
        // POST: LoginController/CambiarClave
        [HttpPost]
        public async Task<IActionResult> CambiarClave(CambioClaveVM modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                var httpClient = new HttpClient();
                var url = $"https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/ObtengaElUsuarioPorNombre/{modelo.Nombre}";
                Usuario usuario = await httpClient.GetFromJsonAsync<Usuario>(url);

                if (usuario == null)
                {
                    ViewData["Mensaje"] = "Usuario no encontrado.";
                    return View(modelo);
                }

                string laClaveNueva = modelo.ClaveNueva;

                var respuesta = await httpClient.PutAsJsonAsync($"https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/CambieLaClave/{laClaveNueva}", usuario);

                if (respuesta.StatusCode != HttpStatusCode.OK)
                {
                    ViewData["Mensaje"] = "Error al cambiar la clave.";
                    return View(modelo);
                }

                string asunto = "Cambio de clave";
                string contenido = $"Le informamos que el cambio de clave de la cuenta del usuario {modelo.Nombre} se ejecutó satisfactoriamente.";

                var respuestaCorreo = await httpClient.PostAsJsonAsync("https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/EnvieElCorreoElectronico", new { correo = usuario.correoElectronico, asunto, contenido });

                if (respuestaCorreo.StatusCode != HttpStatusCode.OK)
                {
                    ViewData["Mensaje"] = "Error al enviar el correo electrónico.";
                    return View(modelo);
                }

                ViewData["Mensaje"] = "Cambio de clave realizado correctamente.";
                return View(modelo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                ViewData["Mensaje"] = "Ocurrió un error al cambiar la clave.";
                return View(modelo);
            }
        }


        public IActionResult SolicitudPendiente()
        {
            return View();
        }


        // GET: SolicitudDeRegistrosController
        public async Task<IActionResult> Index(string nombre)
        {

            List<Model.SolicitudRegistro> lista;

            var httpClient = new HttpClient();

            try
            {
                var respuesta = await httpClient.GetAsync("https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/ObtengaLaListaDePendientes");
                string apiResponse = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.SolicitudRegistro>>(apiResponse);

                if (nombre is null)
                {
                    return View(lista);
                }
                else
                {
                    List<Model.SolicitudRegistro> listaFiltrada;
                    listaFiltrada = lista.Where(x => x.Nombre.Contains(nombre)).ToList();
                    return View(listaFiltrada);
                }
            }
            catch (Exception ex)
            {
                return View();
            }

        }
        
        public async Task<IActionResult> Activar(int id)
        {

            try
            {
                var httpClient = new HttpClient();

                int solicitudId = id;

                var uri = $"https://apicomerciovs.azurewebsites.net/ModuloLoginRegistro/AprobarSolicitud/{solicitudId}";
                var respuesta = await httpClient.PutAsync(uri, null);
                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                return View();
            }

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
