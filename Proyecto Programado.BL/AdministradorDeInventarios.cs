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

        public void AgregueelInventario(Model.Inventario inventario, string nombreUsuario)
        {
            inventario.Cantidad = 0;
            ElContexto.Inventarios.Add(inventario);
            ElContexto.SaveChanges();

            var historico = new HistoricoInventario
            {
                IdInventario = inventario.Id,
                nombreUsuario = nombreUsuario,
                fechaCreacion = DateTime.UtcNow,
                TipoModificacion = TipoModificacion.Creacion,
                
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

        public (Inventario, List<HistoricoInventario>) ObtengaInventarioConHistorico(int id)
        {
            var inventario = ElContexto.Inventarios.Find(id);


            var historicoInventarios = ElContexto.HistoricoInventario
                                               .Where(h => h.IdInventario == id)
                                               .ToList();

            return (inventario, historicoInventarios);
        }




        public void EditeElInventario(Model.Inventario inventario, string nombreUsuario)
        {
            Model.Inventario inventarioOriginal = ObtengaElInventario(inventario.Id);

            List<string> modificaciones = new List<string>();

            if (inventarioOriginal.Nombre != inventario.Nombre)
            {
                modificaciones.Add($"Nombre: {inventarioOriginal.Nombre} -> {inventario.Nombre}");
            }

            if (inventarioOriginal.Categoria != inventario.Categoria)
            {
                modificaciones.Add($"Categoría: {inventarioOriginal.Categoria} -> {inventario.Categoria}");
            }

            if (inventarioOriginal.Precio != inventario.Precio)
            {
                modificaciones.Add($"Precio: {inventarioOriginal.Precio} -> {inventario.Precio}");
            }

            inventarioOriginal.Nombre = inventario.Nombre;
            inventarioOriginal.Categoria = inventario.Categoria;
            inventarioOriginal.Precio = inventario.Precio;

            ElContexto.Inventarios.Update(inventarioOriginal);
            ElContexto.SaveChanges();

            var historico = new HistoricoInventario
            {
                IdInventario = inventario.Id,
                nombreUsuario = nombreUsuario,
                fechaCreacion = DateTime.UtcNow,
                TipoModificacion = TipoModificacion.Modificacion,
                Modificacion = string.Join(" | ", modificaciones) 
            };

            ElContexto.HistoricoInventario.Add(historico);
            ElContexto.SaveChanges();
        }

    }
}
