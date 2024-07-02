using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programado.DA;
using Proyecto_Programado.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Programado.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuloDeCajasController : ControllerBase
    {
        public DBContexto ElContexto;
        private readonly BL.IAdministradorDeCaja ElAdministrador;
        public ModuloDeCajasController(Proyecto_Programado.BL.IAdministradorDeCaja administrador, DBContexto elContexto)
        {
            ElAdministrador = administrador;
            ElContexto = elContexto;
        }
        [HttpPost("AbraLaCaja")]
        public IActionResult AbraLaCaja([FromBody] AperturaDeCaja laCaja, [FromQuery] string elNombreDeUsuario)
        {
            if (laCaja == null || string.IsNullOrEmpty(elNombreDeUsuario))
            {
                return BadRequest("Datos inválidos");
            }

            laCaja.UserId = elNombreDeUsuario;
            laCaja.FechaDeInicio = DateTime.Now;
            laCaja.Estado = EstadoCajas.Abierta;
            laCaja.Efectivo = 0;
            laCaja.Tarjeta = 0;
            laCaja.SinpeMovil = 0;

            ElContexto.AperturasDeCaja.Add(laCaja);
            ElContexto.SaveChanges();

            return Ok(laCaja);
        }

        [HttpPost("CierreLaCaja/{elId}")]
        public IActionResult CierreLaCaja(int elId)
        {
            var laCaja = ElContexto.AperturasDeCaja.FirstOrDefault(elElemento => elElemento.Id == elId);

            if (laCaja == null)
            {
                return NotFound("La caja no fue encontrada");
            }

            laCaja.FechaDeCierre = DateTime.Now;
            laCaja.Estado = EstadoCajas.Cerrada;

            ElContexto.SaveChanges();

            return Ok(laCaja);
        }

        [HttpGet("ObtengaLaCajaAbierta")]
        public IActionResult ObtengaLaCajaAbierta([FromQuery] string elNombreDeUsuario)
        {
            if (string.IsNullOrEmpty(elNombreDeUsuario))
            {
                return BadRequest("Nombre de usuario inválido");
            }

            var laCaja = ElContexto.AperturasDeCaja.FirstOrDefault(elElemento => elElemento.UserId == elNombreDeUsuario && elElemento.Estado == EstadoCajas.Abierta);

            if (laCaja == null)
            {
                return NotFound("No se encontró una caja abierta para el usuario proporcionado");
            }

            return Ok(laCaja);
        }
    }
}
