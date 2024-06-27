using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Proyecto_Programado.UI.ViewModels

{
    public class VentaDetalleVM
    {
        public int Id { get; set; }
        public int Id_Venta { get; set; }
        public int Id_Inventario { get; set; }
        [Display(Name = "Nombre del item")]
        public string NombreInventario { get; set; }
        public int Cantidad { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Precio { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "Subtotal")]
        public decimal Monto { get; set; }

        public int PorcentajeDescuento { get; set; }

        [Display(Name = "Monto descuento")]
        [Range(0, 100, ErrorMessage = "El valor debe entre 0 y 100.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal MontoDescuento { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Total { get; set; }
    }
}