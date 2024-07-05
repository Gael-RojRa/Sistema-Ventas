using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programado.DA;
using Proyecto_Programado.Model;

namespace Proyecto_Programado.SI.Controllers
{
    public class ModuloDeVentasController : ControllerBase
    {
        public DBContexto ElContexto;
        private readonly BL.IAdministradorDeVentas ElAdministrador;
        public ModuloDeVentasController(Proyecto_Programado.BL.IAdministradorDeVentas administrador, DBContexto elContexto)
        {
            ElAdministrador = administrador;
            ElContexto = elContexto;
        }

        [HttpPost("AgregueDetalleVenta")]
        public IActionResult AgregueDetalleVenta([FromBody] VentaDetalles elNuevoDetalleDeVenta)
        {
            if (elNuevoDetalleDeVenta == null)
            {
                return BadRequest("El detalle de venta no puede ser nulo.");
            }

            ElContexto.VentaDetalles.Add(elNuevoDetalleDeVenta);
            ElContexto.SaveChanges();

            return Ok("Detalle de venta agregado exitosamente.");
        }

        // GET: api/Ventas/5
        [HttpGet("ObtengaElNombreDeVenta/{id}")]
        public ActionResult<string> ObtengaElNombreDeVenta(int id)
        {
            var nombreVenta = ElAdministrador.ObtengaElNombreDeVenta(id);

            if (string.IsNullOrEmpty(nombreVenta))
            {
                return NotFound();
            }

            return nombreVenta;
        }
        [HttpPost("AgregueLaVenta")]
        public IActionResult AgregueLaVenta([FromBody] Model.Venta venta)
        {
            ElAdministrador.AgregueLaVenta(venta);
            return Ok(venta);
        }
        // GET: api/Ventas/Inventario/5
        [HttpGet("ObtengaElInventario/{id}")]
        public ActionResult<Inventario> ObtengaElInventario(int id)
        {
            var inventario = ElAdministrador.ObtengaElInventario(id);

            if (inventario == null)
            {
                return NotFound();
            }

            return inventario;
        }
        // GET: api/Ventas/Detalles/5
        [HttpGet("ObtengaLosItemsDeUnaVenta/{id}")]
        public ActionResult<List<VentaDetalles>> ObtengaLosItemsDeUnaVenta(int id)
        {
            var detalles = ElAdministrador.ObtengaLosItemsDeUnaVenta(id);

            if (detalles == null || !detalles.Any())
            {
                return NotFound();
            }

            return detalles;
        }
        // GET: api/Ventas/Inventarios
        [HttpGet("ObtengaLaListaDeInventarios")]
        public ActionResult<List<Inventario>> ObtengaLaListaDeInventarios()
        {
            return ElAdministrador.ObtengaLaListaDeInventarios();
        }
        // GET: api/Ventas
        [HttpGet("ObtengaLaListaDeVentas")]
        public ActionResult<List<Venta>> ObtengaLaListaDeVentas()//llamada en controler
        {
            return ElAdministrador.ObtengaLaListaDeVentas();
        }
        // GET: api/Ventas/CajaAbierta/{nombreUsuario}
        [HttpGet("ObtengaElIdDeLaCajaAbierta/{nombreUsuario}")]
        public ActionResult<int> ObtengaElIdDeLaCajaAbierta(string nombreUsuario)
        {
            var idCaja = ElAdministrador.ObtengaElIdDeLaCajaAbierta(nombreUsuario);

            if (idCaja == 0)
            {
                return NotFound();
            }

            return idCaja;
        }
        // GET: api/Ventas/PrecioInventario/5
        [HttpGet("ObtengaElPrecioDelInventario/{id}")]
        public ActionResult<decimal> ObtengaElPrecioDelInventario(int id)
        {
            var precio = ElAdministrador.ObtengaElPrecioDelInventario(id);

            if (precio == 0)
            {
                return NotFound();
            }

            return precio;
        }

        [HttpPut("ActualiceLaVenta/{id}")]
        public IActionResult ActualiceLaVenta(int id, [FromBody] Venta laVentaActualizada)
        {
            ElAdministrador.ActualiceLaVenta(id, laVentaActualizada);
            return Ok();
        }


        [HttpPut("ActualiceElTotalEnElIndexDeVentas/{id}")]
        public IActionResult ActualiceElTotalEnElIndexDeVentas(int id)
        {

            ElAdministrador.ActualiceElTotalEnElIndexDeVentas(id);

            return Ok();
        }

        [HttpPut("ApliqueElDescuento/{id}/{porcentajeDescuento}")]
        public IActionResult ApliqueElDescuento(int porcentajeDescuento, int id)
        {
            ElAdministrador.ApliqueElDescuento(porcentajeDescuento, id);

            return Ok();
        }

        [HttpPut("ActualiceLaCantidadDeInventario/{id}/{cantidadVendida}")]
        public IActionResult ActualiceLaCantidadDeInventario(int cantidadVendida, int id)
        {
            ElAdministrador.ActualiceLaCantidadDeInventario(cantidadVendida, id);

            return Ok();
        }

        [HttpPut("RestaureLaCantidadDelItemEliminado/{id}/{cantidadDevuelta}")]
        public IActionResult RestaureLaCantidadDelItemEliminado(int cantidadDevuelta, int id)
        {
            var elInventario = ElContexto.Inventarios.Find(id);
            if (elInventario == null)
            {
                return NotFound();
            }

            elInventario.Cantidad += cantidadDevuelta;
            ElContexto.Inventarios.Update(elInventario);
            ElContexto.SaveChanges();

            return NoContent();
        }

        [HttpGet("ObtengaVentaPorId/{id}")]
        public ActionResult<Venta> ObtengaVentaPorId(int id)
        {
            var venta = ElContexto.Ventas.Find(id);
            if (venta == null)
            {
                return NotFound();
            }

            return Ok(venta);
        }

        [HttpDelete("ElimineLaVenta/{id}")]
        public IActionResult ElimineLaVenta(int id)
        {
            ElAdministrador.ElimineLaVenta(id);

            return Ok();
        }

        [HttpGet("ObtengaVentaDetallePorId/{id}")]
        public ActionResult<VentaDetalles> ObtengaVentaDetallePorId(int id)
        {
            var ventaDetalle = ElContexto.VentaDetalles.Find(id);
            if (ventaDetalle == null)
            {
                return NotFound();
            }

            return Ok(ventaDetalle);
        }

        [HttpGet("VerifiqueLaCajaAbierta/{elNombreUsuario}")]
        public IActionResult VerifiqueLaCajaAbierta(string elNombreUsuario)
        {
            var laCajaAbierta = ElContexto.AperturasDeCaja
                .FirstOrDefault(elElemento => elElemento.UserId == elNombreUsuario && elElemento.Estado == EstadoCajas.Abierta);

            bool laCajaEstaAbierta = (laCajaAbierta != null);
            return Ok(laCajaEstaAbierta);
        }
    }
}
