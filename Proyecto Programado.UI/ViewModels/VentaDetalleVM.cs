using System.ComponentModel.DataAnnotations;
namespace Proyecto_Programado.UI.ViewModels

{
    public class VentaDetalleVM
    {
        public int Id { get; set; }
        public int Id_Venta { get; set; }
        public int Id_Inventario { get; set; }
        public string NombreInventario { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Monto { get; set; }

        [Display(Name = "Monto descuento")]
        public decimal MontoDescuento { get; set; }

        public decimal Total { get; set; }
    }
}