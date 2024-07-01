using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;

namespace Proyecto_Programado.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ModuloUsuarioController : ControllerBase
    {

        private readonly Proyecto_Programado.BL.IAdministradorDeUsuarios ElAdministradorDeUsuarios;

        public ModuloUsuarioController(IAdministradorDeUsuarios elAdministradorDeUsuarioes)
        {
            ElAdministradorDeUsuarios= elAdministradorDeUsuarioes;
        }


        [HttpGet()]
        public IActionResult VerifiqueCredenciales([FromBody] PracticaExam.Model.Libro elLibro)    }
}
