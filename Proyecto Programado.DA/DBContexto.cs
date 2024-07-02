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
        public DbSet<HistoriconInventario> HistoricoInventario { get; set; }
        public DbSet<AjusteDeInventario> AjusteDeInventarios { get; set; }
        public DbSet<AperturaDeCaja> AperturasDeCaja { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalles> VentaDetalles { get; set; }
        public DbSet<SolicitudRegistro> SolicitudRegistro { get; set; }

    }
}
