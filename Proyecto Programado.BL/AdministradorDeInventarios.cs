using Microsoft.EntityFrameworkCore;
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
<<<<<<< HEAD
        public void AgregueelInventario(Model.Inventario inventario, string nombreUsuario)
=======
        public void AgregueElInventario(Model.Inventario inventario)
>>>>>>> f69a59c15a79460f741b924b01beac7cec6da555
        {
            inventario.Cantidad = 0;
            inventario.Precio = Convert.ToDecimal(inventario.Precio.ToString().Replace(',', '.'));
            ElContexto.Inventarios.Add(inventario);
            ElContexto.SaveChanges();

            var historico = new HistoricoInventario
            {
                IdInventario = inventario.Id,
                nombreUsuario = nombreUsuario,
                fechaCreacion = DateTime.UtcNow,
                TipoModificacion = 0,
                
            };


            ElContexto.HistoricoInventario.Add(historico);
            ElContexto.SaveChanges();
            

        }
        public Model.Inventario ObtengaElInventario(int Id)
        {
            Model.Inventario resultado;
            resultado = ElContexto.Inventarios.Find(Id);
            return resultado;
        }
        public void EditeElInventario(Model.Inventario inventario)
        {
            Model.Inventario inventarioAEditar;
            inventarioAEditar = ObtengaElInventario(inventario.Id);
            
            inventarioAEditar.Nombre = inventario.Nombre;
            inventarioAEditar.Categoria = inventario.Categoria;
            inventarioAEditar.Precio = inventario.Precio;

            ElContexto.Inventarios.Update(inventarioAEditar);
            ElContexto.SaveChanges();
        }
    }
}
