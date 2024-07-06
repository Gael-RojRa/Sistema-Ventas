using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectoProgramado.SI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ModuloLoginRegistroController : ControllerBase
    {
        private readonly Proyecto_Programado.BL.IAdministradorDeSolicitudes ElAdministrador;
        private readonly Proyecto_Programado.BL.IAdministradorDeUsuarios    ElAdministradorDeUsuarios;
        public ModuloLoginRegistroController(Proyecto_Programado.BL.IAdministradorDeSolicitudes administrador, Proyecto_Programado.BL.IAdministradorDeUsuarios administrador2)
        {
            ElAdministrador = administrador;
            ElAdministradorDeUsuarios = administrador2;
        }
        // POST: api/SoliciteElRegistro
        [HttpPost("SoliciteElRegistro")]
        public IActionResult SoliciteElRegistro([FromBody] RegistroDTO registro)
        {
            try
            {
                bool resultado = ElAdministrador.SoliciteElRegistro(registro.NombreUsuario, registro.Email, registro.Clave);
                if (resultado)
                {
                    return Ok("Solicitud de registro exitosa");
                }
                else
                {
                    return StatusCode(500, "Error al solicitar el registro");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al solicitar el registro: {ex.Message}");
            }
        }
        // POST: api/EnviarCorreo
        [HttpPost("EnvieElCorreoElectronico")]
        public IActionResult EnvieElCorreoElectronico([FromBody] string elDestinatario, string elAsunto, string elContenido)
        {
            try
            {
                ElAdministrador.EnvieElCorreoElectronico(elDestinatario, elAsunto, elContenido);
                return Ok("Correo electrónico enviado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar el correo electrónico: {ex.Message}");
            }
        }
        // POST: api/AprobarSolicitud
        [HttpPut("AprobarSolicitud/{solicitudId}")]
        public IActionResult AprobarSolicitud(int solicitudId)
        { 
               ElAdministrador.AprobarSolicitud(solicitudId);
               return Ok();
        }
   
       
        // GET: api/ObtenerSolicitud/{id}
        [HttpGet("ObtengaLaSolicitud/{id}")]
        public IActionResult ObtengaLaSolicitud(int id)
        {
            try
            {
                var solicitud = ElAdministrador.ObtengaLaSolicitud(id);

                if (solicitud == null)
                {
                    return NotFound("Solicitud no encontrada");
                }

                return Ok(solicitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la solicitud: {ex.Message}");
            }
        }
        // POST: api/CrearUsuario
        [HttpPost("CrearUsuario")]
        public IActionResult CrearUsuario([FromBody] string usuario, string correoElectronico)
        {
            try
            {
                ElAdministrador.CrearUsuario(usuario, correoElectronico);
                return Ok("Usuario creado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el usuario: {ex.Message}");
            }
        }
        // GET: api/ObtengaElUsuarioPorNombreEnSolicitudes/{nombre}
        [HttpGet("ObtengaElUsuarioPorNombreEnSolicitudes/{nombre}")]
        public IActionResult ObtengaElUsuarioPorNombreEnSolicitudes(string nombre)
        {
            try
            {
                var usuario = ElAdministrador.ObtengaElUsuarioPorNombre(nombre);

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el usuario: {ex.Message}");
            }
        }

        // GET: api/ObtenerUsuarioPorNombre/{nombre}
        [HttpGet("ObtengaElUsuarioPorNombre/{nombre}")]
        public IActionResult ObtengaElUsuarioPorNombre(string nombre)
        {
            try
            {
                var usuario = ElAdministradorDeUsuarios.ObtengaElUsuarioPorNombre(nombre);

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el usuario: {ex.Message}");
            }
        }

        // GET: api/ObtenerRolDelUsuario/{nombre}
        [HttpGet("ObtengaElRolDelUsuario/{nombre}")]
        public IActionResult ObtengaElRolDelUsuario(string nombre)
        {
            try
            {
                var rol = ElAdministrador.ObtengaElRolDelUsuario(nombre);

                if (rol == null)
                {
                    return NotFound("Usuario no encontrado o sin rol asignado");
                }

                return Ok(rol);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el rol del usuario: {ex.Message}");
            }
        }
        // GET: api/ObtenerListaDeSolicitudes
        [HttpGet("ObtengaLaListaDeSolicitudes")]
        public IActionResult ObtengaLaListaDeSolicitudes()
        {
            try
            {
                var listaDeSolicitudes = ElAdministrador.ObtengaLaListaDeSolicitudes();

                if (listaDeSolicitudes == null || !listaDeSolicitudes.Any())
                {
                    return NotFound("No se encontraron solicitudes de registro");
                }

                return Ok(listaDeSolicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de solicitudes: {ex.Message}");
            }
        }
        // GET: api/ObtengaLaLista
        [HttpGet("ObtengaLaLista")]
        public IActionResult ObtengaLaLista()
        {
            try
            {
                var listaDeSolicitudes = ElAdministrador.ObtengaLaLista();

                if (listaDeSolicitudes == null || !listaDeSolicitudes.Any())
                {
                    return NotFound("No se encontraron solicitudes de registro");
                }

                return Ok(listaDeSolicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de solicitudes: {ex.Message}");
            }
        }
        // GET: api/ObtengaLaListaDePendientes
        [HttpGet("ObtengaLaListaDePendientes")]
        public IActionResult ObtengaLaListaDePendientes()
        {
            return Ok(ElAdministrador.ObtengaLaListaDePendientes());
        }

        ////////////////
        // POST: api/Ventas/EnviarCorreo
        [HttpPost("EnviarCorreo")]
        public IActionResult EnviarCorreo([FromBody] string elDestinatario, string elAsunto, string elContenido)
        {
            try
            {
                ElAdministrador.EnvieElCorreoElectronico(elDestinatario, elAsunto, elContenido);
                return Ok("Correo enviado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar el correo electrónico: {ex.Message}");
            }
        }

        // POST: api/VerifiqueCredenciales
        [HttpPost("VerifiqueCredenciales")]
        public ActionResult<bool> VerifiqueCredenciales([FromBody] VerificarCredencialesDTO credenciales)
        {
            try
            {
                bool resultado = ElAdministradorDeUsuarios.VerifiqueCredenciales(credenciales.NombreUsuario, credenciales.Clave);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar las credenciales: {ex.Message}");
            }
        }




        // PUT: api/Ventas/CambiarClave
        [HttpPut("CambieLaClave")]
        public IActionResult CambieLaClave([FromBody] Usuario elUsuario, string laClaveNueva)
        {
            try
            {
                var usuario = ElAdministradorDeUsuarios.ObtengaElUsuarioPorNombre(elUsuario.Nombre);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                ElAdministradorDeUsuarios.CambieLaClave(usuario, laClaveNueva);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cambiar la clave: {ex.Message}");
            }
        }
    }
}

