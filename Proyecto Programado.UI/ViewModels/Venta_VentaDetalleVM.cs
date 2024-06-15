using Proyecto_Programado.Model;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programado.UI.ViewModels
{
    public class Venta_VentaDetalleVM
    {
        [Required]
        public string NombreCliente { get; set; }
        public List<Inventario> ItemsInventario { get; set; }
        public int IdItemSeleccionado { get; set; } 
        public int Cantidad { get; set; }
        public int idVenta { get; set; }

    }
}
