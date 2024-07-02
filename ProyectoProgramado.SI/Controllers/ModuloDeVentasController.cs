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
            Venta ventaOriginal = ElContexto.Ventas.Find(id);
            if (ventaOriginal == null)
            {
                return NotFound();
            }

            ventaOriginal.SubTotal = laVentaActualizada.SubTotal;
            ventaOriginal.Total = laVentaActualizada.Total;
            ventaOriginal.MontoDescuento = laVentaActualizada.MontoDescuento;
            ventaOriginal.PorcentajeDescuento = laVentaActualizada.PorcentajeDescuento;

            AperturaDeCaja laAperturaDeCaja = ElContexto.AperturasDeCaja.Find(laVentaActualizada.IdAperturaDeCaja);
            if (laAperturaDeCaja == null)
            {
                return NotFound();
            }

            TipoDePago tipoDePago = laVentaActualizada.TipoDePago;

            if (tipoDePago == TipoDePago.Efectivo)
            {
                laAperturaDeCaja.Efectivo += laVentaActualizada.Total;
            }
            else if (tipoDePago == TipoDePago.Tarjeta)
            {
                laAperturaDeCaja.Tarjeta += laVentaActualizada.Total;
            }
            else if (tipoDePago == TipoDePago.SinpeMovil)
            {
                laAperturaDeCaja.SinpeMovil += laVentaActualizada.Total;
            }

            ElContexto.AperturasDeCaja.Update(laAperturaDeCaja);
            ElContexto.Ventas.Update(ventaOriginal);
            ElContexto.SaveChanges();

            return NoContent();
        }


        [HttpPut("ActualiceElTotalEnElIndexDeVentas/{id}")]
        public IActionResult ActualiceElTotalEnElIndexDeVentas(int id)
        {
            var elDetalleActual = ElContexto.VentaDetalles.Find(id);
            if (elDetalleActual == null)
            {
                return NotFound();
            }

            var laSumatoriaDelMontoDeDetalles = ElContexto.VentaDetalles
                .Where(elElemento => elElemento.Id_Venta == elDetalleActual.Id_Venta)
                .Sum(elElemento => elElemento.Monto);

            var laVentaAModificar = ElContexto.Ventas.Find(elDetalleActual.Id_Venta);
            if (laVentaAModificar == null)
            {
                return NotFound();
            }

            laVentaAModificar.SubTotal = laSumatoriaDelMontoDeDetalles;
            laVentaAModificar.Total = laSumatoriaDelMontoDeDetalles;

            ElContexto.Ventas.Update(laVentaAModificar);
            ElContexto.SaveChanges();

            return NoContent();
        }

        [HttpPut("apliqueDescuento/{id}/{porcentajeDescuento}")]
        public IActionResult ApliqueElDescuento(int porcentajeDescuento, int id)
        {
            decimal elPorcentajeDecimal = porcentajeDescuento / 100.0m;

            ElContexto.Database.ExecuteSqlRaw(
                "UPDATE VentaDetalles " +
                "SET MontoDescuento = Monto * {0}, " +
                "MontoFinal = Monto - (Monto * {0}) " +
                "WHERE Id_Venta = {1}",
                elPorcentajeDecimal, id);

            ElContexto.Database.ExecuteSqlRaw(
                "UPDATE Ventas " +
                "SET PorcentajeDescuento = {2}, " +
                "MontoDescuento = SubTotal * {0}, " +
                "Total = SubTotal - (SubTotal * {0}) " +
                "WHERE Id = {1}",
                elPorcentajeDecimal, id, porcentajeDescuento);

            return NoContent();
        }
        [HttpPut("ActualiceLaCantidadDeInventario/{id}/{cantidadVendida}")]
        public IActionResult ActualiceLaCantidadDeInventario(int cantidadVendida, int id)
        {
            var elInventario = ElContexto.Inventarios.Find(id);
            if (elInventario == null)
            {
                return NotFound();
            }

            elInventario.Cantidad -= cantidadVendida;
            if (elInventario.Cantidad < 0)
            {
                return BadRequest("La cantidad en el inventario no puede ser negativa.");
            }

            ElContexto.Inventarios.Update(elInventario);
            ElContexto.SaveChanges();

            return NoContent();
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
            var elItemAEliminar = ElContexto.VentaDetalles.Find(id);
            if (elItemAEliminar == null)
            {
                return NotFound();
            }

            ElContexto.VentaDetalles.Remove(elItemAEliminar);
            ElContexto.SaveChanges();

            int idDeLaVentaDelDetalle = elItemAEliminar.Id_Venta;
            var laVentaParaActualizar = ElContexto.Ventas.Find(idDeLaVentaDelDetalle);
            if (laVentaParaActualizar == null)
            {
                return NotFound();
            }

            var laSumatoriaDelMontoDeDetalles = ElContexto.VentaDetalles
                .Where(elElemento => elElemento.Id_Venta == laVentaParaActualizar.Id)
                .Sum(elElemento => elElemento.Monto);

            laVentaParaActualizar.SubTotal = laSumatoriaDelMontoDeDetalles;
            decimal porcentajeDescuentoDecimal = laVentaParaActualizar.PorcentajeDescuento / 100m;
            decimal valorDescuento = laSumatoriaDelMontoDeDetalles * porcentajeDescuentoDecimal;
            laVentaParaActualizar.Total = laSumatoriaDelMontoDeDetalles - valorDescuento;
            laVentaParaActualizar.MontoDescuento = valorDescuento;

            ElContexto.Ventas.Update(laVentaParaActualizar);
            ElContexto.SaveChanges();

            if (laVentaParaActualizar.SubTotal == 0)
            {
                ElContexto.Ventas.Remove(laVentaParaActualizar);
                ElContexto.SaveChanges();
            }

            return NoContent();
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
