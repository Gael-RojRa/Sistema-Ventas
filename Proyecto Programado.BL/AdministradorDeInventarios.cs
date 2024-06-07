using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeInventarios : IAdministradorDeInventarios
    {

        public DBContexto ElContexto;

        public AdministradorDeInventarios(DBContexto contexto)
        {
            ElContexto = contexto;
        }

        public List<Inventario> ObtenLaListaDeInventarios()
        {

            var laListaDeInventarios = ElContexto.Inventarios.ToList();

            return laListaDeInventarios;
        }
        public void AgregueelInventario(Model.Inventario inventario)
        {
            inventario.Cantidad = 0;
            ElContexto.Inventarios.Add(inventario);
            ElContexto.SaveChanges();
        }
        public Model.Inventario ObtengaElInventario(int id)
        {
            Model.Inventario resultado;
            resultado = ElContexto.Inventarios.Find(id);
            return resultado;
        }
        public void EditeElInventario(Model.Inventario inventario)
        {
            //Model.Inventario InventarioAEditar;
            ////InventarioAEditar.ObtengaElInventario(inventario.Id);
            //InventarioAEditar.Nombre = inventario.Nombre;
            //InventarioAEditar.Categoria = inventario.Categoria;
            //InventarioAEditar.Precio = inventario.Precio;


        }
    }
}
