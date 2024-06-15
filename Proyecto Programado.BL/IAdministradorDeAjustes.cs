using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public interface IAdministradorDeAjustes
    {
        public List<Inventario> ObtengaLaListaDeInventarios();

        public List<AjusteDeInventario> ObtengaLaListaDeAjuste();

        public void AgregueUnAjuste(Model.AjusteDeInventario elAjuste, string elNombreDeUsuario);

        public int ObtengaLaCantidadActual(int elId);

        public Model.Inventario ObtengaElInventarioAModificarLaCantidad(int elId);

        public Model.AjusteDeInventario ObtengaLosAjustesDeInventario(int elId);
        public List<Model.AjusteDeInventario> ObtengaLosAjustes(int elId);






    }
}
