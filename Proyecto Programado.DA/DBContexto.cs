using Microsoft.EntityFrameworkCore;
using Proyecto_Programado.Model;

namespace Proyecto_Programado.DA
{
    public class DBContexto : DbContext
    {

        public DBContexto(DbContextOptions<DBContexto> opciones) : base(opciones)
        {
            

        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<HistoricoInventario> HistoricoInventario { get; set; }
        public DbSet<AjusteDeInventario> AjusteDeInventarios { get; set; }
        public DbSet<AperturaDeCaja> AperturasDeCaja { get; set; }
        public DbSet<Ventas> Ventas { get; set; }


    }
}
