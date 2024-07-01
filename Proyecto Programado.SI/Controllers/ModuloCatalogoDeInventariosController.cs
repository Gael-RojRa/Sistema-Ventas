using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;

namespace Proyecto_Programado.SI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuloCatalogoDeInventariosController : Controller
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

        [HttpGet("ObtengaInventarioConHistorico")]
        public IActionResult ObtengaInventarioConHistorico(int elId)
        {
            return Ok(ElAdministradorDeInventarios.ObtengaInventarioConHistorico(elId));
        }
    }
}
