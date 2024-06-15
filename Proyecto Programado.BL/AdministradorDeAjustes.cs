using Proyecto_Programado.DA;
using Proyecto_Programado.Model;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeAjustes : IAdministradorDeAjustes
    {
        public DBContexto ElContexto;

        public AdministradorDeAjustes(DBContexto elContexto)
        {
            ElContexto = elContexto;
        }

        public List<Inventario> ObtengaLaListaDeInventarios()
        {
            var laListaDeInventarios = ElContexto.Inventarios.ToList();

            return laListaDeInventarios;
        }

        public List<AjusteDeInventario> ObtengaLaListaDeAjuste()
        {
            var laListaDeAjusteInventario = ElContexto.AjusteDeInventarios.ToList();

            return laListaDeAjusteInventario;
        }


        public Model.AjusteDeInventario ObtengaLosAjustesDeInventario(int elId)
        {
            var elResultado = ElContexto.AjusteDeInventarios.Find(elId);

            return elResultado;

        }


        public List<Model.AjusteDeInventario> ObtengaLosAjustes(int elId)
        {
            List<Model.AjusteDeInventario> losAjustesEncontrados = new List<Model.AjusteDeInventario>();


            List<Model.AjusteDeInventario> laLista = ObtengaLaListaDeAjuste();


            foreach (var elAjusteDeInventario in laLista)
            {
                if (elAjusteDeInventario.Id_Inventario == elId)
                {
                    losAjustesEncontrados.Add(elAjusteDeInventario);
                }
            }


            return losAjustesEncontrados;
        }






        public void AgregueUnAjuste(Model.AjusteDeInventario elAjuste, string elNombreDeUsuario)
        {
            elAjuste.UserId = elNombreDeUsuario;
            elAjuste.Fecha = DateTime.Now;
            ElContexto.AjusteDeInventarios.Add(elAjuste);
            ElContexto.SaveChanges();

            List<string> laModificacion = new List<string>(); ;

            Model.Inventario elInventario = ObtengaElInventarioAModificarLaCantidad(elAjuste.Id_Inventario);

            if (elAjuste.Tipo == Model.TipoAjuste.Aumento)
            {
                laModificacion.Add($"Cantidad: {elInventario.Cantidad} -> {elInventario.Cantidad + elAjuste.Ajuste}");
                elInventario.Cantidad = elInventario.Cantidad + elAjuste.Ajuste;

            }
            else
            {
                laModificacion.Add($"Cantidad: {elInventario.Cantidad} -> {elInventario.Cantidad - elAjuste.Ajuste}");
                elInventario.Cantidad = elInventario.Cantidad - elAjuste.Ajuste;
            }
            ElContexto.Inventarios.Update(elInventario);
            ElContexto.SaveChanges();

            var elHistorico = new HistoricoInventario
            {
                IdInventario = elInventario.Id,
                nombreUsuario = elNombreDeUsuario,
                fechaCreacion = DateTime.UtcNow,
                TipoModificacion = TipoModificacion.Modificacion,
                Modificacion = string.Join("\n", laModificacion)
            };
            ElContexto.HistoricoInventario.Add(elHistorico);
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
