using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Programado.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuloDeSolicitudDeRegistroController : ControllerBase
    {
        private readonly BL.IAdministradorDeSolicitudes ElAdministrador;
        public ModuloDeSolicitudDeRegistroController(Proyecto_Programado.BL.IAdministradorDeSolicitudes administrador)
        {
            ElAdministrador = administrador;
        }
        // POST: api/SoliciteElRegistro
        [HttpPost("SoliciteElRegistro")]
        public IActionResult SoliciteElRegistro([FromBody] string nombreUsuario, string email, string clave)
        {
            try
            {
                bool resultado = ElAdministrador.SoliciteElRegistro(nombreUsuario, email, clave);
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
        [HttpPost("AprobarSolicitud")]
        public IActionResult AprobarSolicitud([FromBody] int solicitudId)
        {
            try
            {
                bool resultado = ElAdministrador.AprobarSolicitud(solicitudId);

                if (resultado)
                {
                    return Ok("Solicitud aprobada exitosamente");
                }
                else
                {
                    return NotFound("Solicitud no encontrada");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al aprobar la solicitud: {ex.Message}");
            }
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
        // GET: api/ObtenerUsuarioPorNombre/{nombre}
        [HttpGet("ObtengaElUsuarioPorNombre/{nombre}")]
        public IActionResult ObtengaElUsuarioPorNombre(string nombre)
        {
            try
            {
                var solicitud = ElAdministrador.ObtengaElUsuarioPorNombre(nombre);

                if (solicitud == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                return Ok(solicitud);
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
        // GET: api/ObtenerListaDePendientes
        [HttpGet("ObtengaLaListaDePendientes")]
        public IActionResult ObtengaLaListaDePendientes()
        {
            try
            {
                var listaDePendientes = ElAdministrador.ObtengaLaListaDePendientes();

                if (listaDePendientes == null || !listaDePendientes.Any())
                {
                    return NotFound("No se encontraron solicitudes pendientes");
                }

                return Ok(listaDePendientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de solicitudes pendientes: {ex.Message}");
            }
        }
    }
}
