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

           public AperturaDeCaja ObtenerApertura()
            {

                var apertura = ElContexto.AperturasDeCaja.FirstOrDefault(a => a.Estado == 1);
                if (apertura != null)
                {
                    if (apertura.FechaDeCierre.HasValue)
                    {
                        
                    }
                }
                return apertura; 
            }
    
        public void AbrirCaja(string UserId)
        {
            var apertura = new AperturaDeCaja
            {
                UserId = UserId,
                FechaDeInicio = DateTime.Now,
                Estado = 1
            };
            ElContexto.AperturasDeCaja.Add(apertura);
            ElContexto.SaveChanges();
        }

        /* public void CerrarCaja()
        {
            var apertura = ElContexto.AperturasDeCaja.FirstOrDefault(a => a.Estado == 1);
            if (apertura != null)
            {
                apertura.FechaDeCierre = DateTime.Now;
                apertura.Estado = 0;
                ElContexto.SaveChanges();
            }
        } */
    }
}

