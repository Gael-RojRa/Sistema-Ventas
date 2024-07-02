using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;

namespace Proyecto_Programado.SI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuloCatalogoDeInventariosController : ControllerBase
    {
        private readonly Proyecto_Programado.BL.IAdministradorDeInventarios ElAdministradorDeInventarios;

        public ModuloCatalogoDeInventariosController(IAdministradorDeInventarios elAdministradorDeInventarios)
        {

            ElAdministradorDeInventarios = elAdministradorDeInventarios;

        }

        [HttpGet("ObtengaLaListaDeInventarios")]
        public IActionResult ObtengaLaListaDeInventarios()
        {
            return Ok(ElAdministradorDeInventarios.ObtengaLaListaDeInventarios());
        }

        [HttpPost("AgregueElInventario")]
        public IActionResult AgregueElInventario([FromBody] Proyecto_Programado.Model.Inventario elInventario, string elNombreDeUsuario)
        {
            ElAdministradorDeInventarios.AgregueElInventario(elInventario, elNombreDeUsuario);
            return Ok();
        }


        [HttpGet("ObtengaElInventario")]
        public IActionResult ObtengaElInventario(int elId)
        {
            return Ok(ElAdministradorDeInventarios.ObtengaElInventario(elId));
        }

        [HttpPost("EditeElInventario")]
        public IActionResult EditeElInventario([FromBody] Proyecto_Programado.Model.Inventario elInventario, string elNombreDeUsuario)
        {
            ElAdministradorDeInventarios.EditeElInventario(elInventario, elNombreDeUsuario);
            return Ok();
        }

        [HttpGet("ObtengaInventarioConHistorico/{elId}")]
        public IActionResult ObtengaInventarioConHistorico(int elId)
        {
            var respuesta = ElAdministradorDeInventarios.ObtengaInventarioConHistorico(elId);

           

            return Ok(new { Inventario = respuesta.Item1, HistoricoInventarios = respuesta.Item2 });
        }

    }
}
