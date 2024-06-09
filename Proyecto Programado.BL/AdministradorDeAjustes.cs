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

        public void AgregueUnAjuste(Model.AjusteDeInventario ajuste)
        {
            ElContexto.AjusteDeInventarios.Add(ajuste);
            ElContexto.SaveChanges();

        }

        public int ObtengaLaCantidadActual(int Id)
        {
            var resultado = ElContexto.AjusteDeInventarios.Find(Id);

           return resultado.CantidadActual; 
        }

    }
}
