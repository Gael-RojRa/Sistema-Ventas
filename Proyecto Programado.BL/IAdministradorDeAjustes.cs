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

        public void AgregueUnAjuste(Model.AjusteDeInventario ajuste, string nombreUsuario);

        public int ObtengaLaCantidadActual(int Id);

    }
}
