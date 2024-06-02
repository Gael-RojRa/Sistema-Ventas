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


    }
}
