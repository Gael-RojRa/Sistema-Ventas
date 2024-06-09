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
<<<<<<< HEAD
        void AgregueelInventario(Model.Inventario inventario,string nombreUsuario);
=======

        void AgregueElInventario(Model.Inventario inventario);

        public Model.Inventario ObtengaElInventario(int id);

        public void EditeElInventario(Model.Inventario inventario);
>>>>>>> f69a59c15a79460f741b924b01beac7cec6da555
    }
}
