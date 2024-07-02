using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programado.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Programado.SI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuloDeCajasController : ControllerBase
    {

        private readonly BL.IAdministradorDeCaja ElAdministrador;
        public ModuloDeCajasController(Proyecto_Programado.BL.IAdministradorDeCaja administrador)
        {
            ElAdministrador = administrador;
            
        }
        [HttpPost("AbraLaCaja")]
        public IActionResult AbraLaCaja([FromBody] AperturaDeCaja laCaja, [FromQuery] string elNombreDeUsuario)
        {

            ElAdministrador.AbraLaCaja(laCaja, elNombreDeUsuario);
            return Ok(laCaja);
        }

        [HttpPut("CierreLaCaja/{elId}")]
        public IActionResult CierreLaCaja(int elId)
        {
            ElAdministrador.CierreLaCaja(elId);

            return Ok();
        }

        [HttpGet("ObtengaLaCajaAbierta")]
        public IActionResult ObtengaLaCajaAbierta([FromQuery] string elNombreDeUsuario)
        {

            var laCaja = ElAdministrador.ObtengaLaCajaAbierta(elNombreDeUsuario);

            if (laCaja == null)
            {
                return Ok();
            }

            return Ok(laCaja);
        }
    }
}
