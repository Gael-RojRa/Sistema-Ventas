using Proyecto_Programado.BL;
using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL

{
    public class AdministradorDeCaja : IAdministradorDeCaja
    {
        private readonly DBContexto ElContexto;

        public AdministradorDeCaja(DBContexto contexto)
        {
            ElContexto = contexto;
        }

        /*    public AperturaDeCaja ObtenerApertura()
            {

                var apertura = ElContexto.AperturasDeCaja.FirstOrDefault(a => a.Estado == 1);
                if (apertura != null)
                {
                    if (apertura.FechaDeCierre.HasValue)
                    {
                        // Manejar caso de FechaDeCierre no nulo
                    }
                }
                return apertura; 
            }
    */
        public void AbrirCaja()
        {
            var apertura = new AperturaDeCaja
            {
                UserId = "currentUserId",
                FechaDeInicio = DateTime.Now,
                Estado = 1
            };
            ElContexto.AperturasDeCaja.Add(apertura);
            ElContexto.SaveChanges();
        }
    }
}

