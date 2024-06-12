using Proyecto_Programado.DA;
using Proyecto_Programado.Model;

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

        public List<AjusteDeInventario> ObtenLaListaDeAjuste()
        {
            var laListaDeAjusteInventario = ElContexto.AjusteDeInventarios.ToList();

            return laListaDeAjusteInventario;
        }

  

       

        public List<Model.AjusteDeInventario> ObtengaLosAjustesDeInventario(int id)
        {
            List<Model.AjusteDeInventario> losAjustesEncontrados = new List<Model.AjusteDeInventario>();

           
            List<Model.AjusteDeInventario> lista = ObtenLaListaDeAjuste();

         
            foreach (var ajusteInventario in lista)
            {
                if (ajusteInventario.Id_Inventario == id)
                {
                    losAjustesEncontrados.Add(ajusteInventario); 
                }
            }

           
            return losAjustesEncontrados;
        }




        public void AgregueUnAjuste(Model.AjusteDeInventario ajuste, string nombreUsuario)
        {
            ajuste.UserId = nombreUsuario;
            ajuste.Fecha = DateTime.Now;
            ElContexto.AjusteDeInventarios.Add(ajuste);
            ElContexto.SaveChanges();

            List<string> laModificacion = new List<string>(); ;

            Model.Inventario inventario = ObtengaElInventarioAModificarLaCantidad(ajuste.Id_Inventario);

            if (ajuste.Tipo == Model.TipoAjuste.Aumento)
            {
                laModificacion.Add($"Cantidad: {inventario.Cantidad} -> {inventario.Cantidad + ajuste.Ajuste}");
                inventario.Cantidad = inventario.Cantidad + ajuste.Ajuste;

            }
            else
            {
                laModificacion.Add($"Cantidad: {inventario.Cantidad} -> {inventario.Cantidad - ajuste.Ajuste}");
                inventario.Cantidad = inventario.Cantidad - ajuste.Ajuste;
            }
            ElContexto.Inventarios.Update(inventario);
            ElContexto.SaveChanges();

            var historico = new HistoricoInventario
            {
                IdInventario = inventario.Id,
                nombreUsuario = nombreUsuario,
                fechaCreacion = DateTime.UtcNow,
                TipoModificacion = TipoModificacion.Modificacion,
                Modificacion = string.Join("\n", laModificacion)
            };
            ElContexto.HistoricoInventario.Add(historico);
            ElContexto.SaveChanges();


        }

        public int ObtengaLaCantidadActual(int Id)
        {
            var resultado = ElContexto.Inventarios.Find(Id);

            return resultado.Cantidad;
        }

        public Model.Inventario ObtengaElInventarioAModificarLaCantidad(int Id)
        {
            Model.Inventario resultado;

            resultado = ElContexto.Inventarios.Find(Id);

            return resultado;
        }

    }
}
