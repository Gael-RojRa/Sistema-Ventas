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
        public List<Inventario> ObtenLaListaDeInventarios();

        public List<AjusteDeInventario> ObtenLaListaDeAjuste();

        public void AgregueUnAjuste(Model.AjusteDeInventario ajuste, string nombreUsuario);

        public int ObtengaLaCantidadActual(int Id);

        public Model.Inventario ObtengaElInventarioAModificarLaCantidad(int Id);

        public Model.AjusteDeInventario ObtengaLosAjustesDeInventario(int id);
        public List<Model.AjusteDeInventario> ObtengaLosAjustes(int id);






    }
}
