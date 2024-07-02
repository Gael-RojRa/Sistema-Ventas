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

        List<Inventario> ObtengaLaListaDeInventarios();

        void AgregueElInventario(Model.Inventario elInventario,string elNombreDeUsuario);


        public Model.Inventario ObtengaElInventario(int elId);

        public void EditeElInventario(Model.Inventario elInventario, string elNombreDeUsuario);

        public (Inventario, List<HistoriconInventario>) ObtengaInventarioConHistorico(int elId);
       

    }
}
