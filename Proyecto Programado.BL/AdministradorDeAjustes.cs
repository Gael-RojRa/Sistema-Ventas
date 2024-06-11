using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeAjustes : IAdministradorDeAjustes
    {
        public DBContexto ElContexto;

        public AdministradorDeAjustes(DBContexto contexto)
        {
            ElContexto = contexto;
        }

        public List<Inventario> ObtenLaListaDeInventarios()
        {
            var laListaDeInventarios = ElContexto.Inventarios.ToList();

            return laListaDeInventarios;
        }

        public void AgregueUnAjuste(Model.AjusteDeInventario ajuste, string nombreUsuario)
        {
            ajuste.UserId = nombreUsuario;
            ajuste.Fecha = DateTime.Now;
            ElContexto.AjusteDeInventarios.Add(ajuste);
            ElContexto.SaveChanges();

            Model.Inventario inventario = ObtengaElInventarioAModifircarLaCantidad(ajuste.Id_Inventario);

            if (ajuste.Tipo == Model.TipoAjuste.Aumento)
            {
                inventario.Cantidad = inventario.Cantidad + ajuste.Ajuste;
            }
            else
            {
                inventario.Cantidad = inventario.Cantidad - ajuste.Ajuste;
            }
            ElContexto.Inventarios.Update(inventario);
            ElContexto.SaveChanges();
        }

        public int ObtengaLaCantidadActual(int Id)
        {
            var resultado = ElContexto.Inventarios.Find(Id);

           return resultado.Cantidad; 
        }

        public Model.Inventario ObtengaElInventarioAModifircarLaCantidad(int Id)
        {
            Model.Inventario resultado;

            resultado = ElContexto.Inventarios.Find(Id);

            return resultado;
        }

    }
}
