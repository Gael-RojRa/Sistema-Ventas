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

        public AdministradorDeInventarios(DBContexto elContexto)
        {
            ElContexto = elContexto;
        }

        public List<Inventario> ObtengaLaListaDeInventarios()
        {

            var laListaDeInventarios = ElContexto.Inventarios.ToList();

            return laListaDeInventarios;
        }

        public void AgregueElInventario(Model.Inventario elInventario, string elNombreDeUsuario)
        {
            elInventario.Cantidad = 0;
            ElContexto.Inventarios.Add(elInventario);
            ElContexto.SaveChanges();

            var elHistorico = new HistoricoInventario
            {
                IdInventario = elInventario.Id,
                nombreUsuario = elNombreDeUsuario,
                fechaCreacion = DateTime.UtcNow,
                TipoModificacion = TipoModificacion.Creacion,
                
            };


            ElContexto.HistoricoInventario.Add(elHistorico);
            ElContexto.SaveChanges();
            

        }
        public Model.Inventario ObtengaElInventario(int elId)
        {
            Model.Inventario elResultado;
      
            elResultado = ElContexto.Inventarios.Find(elId);
          
            return elResultado;
        }

        public (Inventario, List<HistoricoInventario>) ObtengaInventarioConHistorico(int elId)
        {
            var elInventario = ElContexto.Inventarios.Find(elId);


            var elHistoricoInventarios = ElContexto.HistoricoInventario
                                               .Where(elElemento => elElemento.IdInventario == elId)
                                               .ToList();

            return (elInventario, elHistoricoInventarios);
        }




        public void EditeElInventario(Model.Inventario elInventario, string elNombreDeUsuario)
        {
            Model.Inventario elInventarioOriginal = ObtengaElInventario(elInventario.Id);

            List<string> lasModificaciones = new List<string>();

            if (elInventarioOriginal.Nombre != elInventario.Nombre)
            {
                lasModificaciones.Add($"Nombre: {elInventarioOriginal.Nombre} -> {elInventario.Nombre}");
            }

            if (elInventarioOriginal.Categoria != elInventario.Categoria)
            {
                lasModificaciones.Add($"Categoría: {elInventarioOriginal.Categoria} -> {elInventario.Categoria}");
            }

            if (elInventarioOriginal.Precio != elInventario.Precio)
            {
                lasModificaciones.Add($"Precio: {elInventarioOriginal.Precio} -> {elInventario.Precio}");
            }

            elInventarioOriginal.Nombre = elInventario.Nombre;
            elInventarioOriginal.Categoria = elInventario.Categoria;
            elInventarioOriginal.Precio = elInventario.Precio;

            ElContexto.Inventarios.Update(elInventarioOriginal);
            ElContexto.SaveChanges();

            var elHistorico = new HistoricoInventario
            {
                IdInventario = elInventario.Id,
                nombreUsuario = elNombreDeUsuario,
                fechaCreacion = DateTime.UtcNow,
                TipoModificacion = TipoModificacion.Modificacion,
                Modificacion = string.Join("\n", lasModificaciones) 
            };

            ElContexto.HistoricoInventario.Add(elHistorico);
            ElContexto.SaveChanges();
        }

    }
}
