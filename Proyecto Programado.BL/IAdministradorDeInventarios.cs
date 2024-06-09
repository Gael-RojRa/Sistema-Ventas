using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public interface IAdministradorDeInventarios
    {

        List<Inventario> ObtenLaListaDeInventarios();

        void AgregueelInventario(Model.Inventario inventario,string nombreUsuario);


        public Model.Inventario ObtengaElInventario(int id);

        public void EditeElInventario(Model.Inventario inventario, string nombreUsuario);

        public (Inventario, List<HistoricoInventario>) ObtengaInventarioConHistorico(int id);
       

    }
}
