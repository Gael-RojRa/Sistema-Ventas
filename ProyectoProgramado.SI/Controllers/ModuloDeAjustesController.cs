using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;


namespace ProyectoProgramado.SI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ModuloDeAjustesController : ControllerBase
    {
        private readonly Proyecto_Programado.BL.IAdministradorDeAjustes ElAdministradorDeAjustes;

        public ModuloDeAjustesController(IAdministradorDeAjustes elAdministradorDeAjustes)
        {

            ElAdministradorDeAjustes = elAdministradorDeAjustes;

        }

        [HttpGet("ObtengaLaListaDeInventarios")]
        public IActionResult ObtengaLaListaDeInventarios()
        {
            return Ok(ElAdministradorDeAjustes.ObtengaLaListaDeInventarios());
        }

        [HttpGet("ObtengaLosAjustes")]
        public IActionResult ObtengaLosAjustes(int id)
        {
            return Ok(ElAdministradorDeAjustes.ObtengaLosAjustes(id));
        }

        [HttpGet("ObtengaLosAjustesDeInventario")]
        public IActionResult ObtengaLosAjustesDeInventario(int id)
        {
            return Ok(ElAdministradorDeAjustes.ObtengaLosAjustesDeInventario(id));
        }


        [HttpGet("ObtengaLaCantidadActual")]
        public IActionResult ObtengaLaCantidadActual(int id)
        {
            return Ok(ElAdministradorDeAjustes.ObtengaLaCantidadActual(id));
        }

        [HttpPost("AgregueUnAjuste")]
        public IActionResult AgregueUnAjuste([FromBody] Proyecto_Programado.Model.AjusteDeInventario elAjuste, [FromQuery] string elNombreDeUsuario)
        {
            ElAdministradorDeAjustes.AgregueUnAjuste(elAjuste, elNombreDeUsuario);
            return Ok();
        }
      
    }
}

