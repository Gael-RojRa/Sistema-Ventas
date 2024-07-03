using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Programado.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuloDeUsuarioController : ControllerBase
    {
        private readonly BL.IAdministradorDeUsuarios ElAdministrador;
        public ModuloDeUsuarioController(Proyecto_Programado.BL.IAdministradorDeUsuarios administrador)
        {
            ElAdministrador = administrador;
        }

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

        // POST: api/Ventas/VerifiqueCredenciales
        [HttpPost("VerifiqueCredenciales")]
        public ActionResult<bool> VerifiqueCredenciales([FromBody] string elNombre, string laClave)
        {
            try
            {
                bool resultado = ElAdministrador.VerifiqueCredenciales(elNombre, laClave);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar las credenciales: {ex.Message}");
            }
        }

        // GET: api/Ventas/UsuarioPorNombre/{nombre}
        [HttpGet("ObtengaElUsuarioPorNombre/{nombre}")]
        public ActionResult<Usuario> ObtengaElUsuarioPorNombre(string nombre)
        {
            var usuario = ElAdministrador.ObtengaElUsuarioPorNombre(nombre);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }
        // GET: api/Ventas/RolDelUsuario/{nombre}
        [HttpGet("ObtengaElRolDelUsuario/{nombre}")]
        public ActionResult<Rol> ObtengaElRolDelUsuario(string nombre)
        {
            var rol = ElAdministrador.ObtengaElRolDelUsuario(nombre);

            if (rol == null)
            {
                return NotFound();
            }

            return rol;
        }

        // PUT: api/Ventas/CambiarClave
        [HttpPut("CambieLaClave")]
        public IActionResult CambieLaClave([FromBody] Usuario elUsuario, string laClaveNueva)
        {
            try
            {
                var usuario = ElAdministrador.ObtengaElUsuarioPorNombre(elUsuario.Nombre);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                ElAdministrador.CambieLaClave(usuario, laClaveNueva);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cambiar la clave: {ex.Message}");
            }
        }
    }
}
