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

        public void AbrirCaja(Model.AperturaDeCaja caja, string nombreUsuario)
        {
            caja.UserId = nombreUsuario;
            caja.FechaDeInicio = DateTime.Now;
            caja.Estado = EstadoCajas.Abierta;

            ElContexto.AperturasDeCaja.Add(caja);
            ElContexto.SaveChanges();
        }
        public void CerrarCaja(int id)
        {
            var caja = ElContexto.AperturasDeCaja.FirstOrDefault(c => c.Id == id);


            caja.FechaDeCierre = DateTime.Now;
            caja.Estado = EstadoCajas.Cerrada;


            ElContexto.SaveChanges();

        }






        public AperturaDeCaja ObtenerCajaAbierta(string nombreUsuario)
        {

            return ElContexto.AperturasDeCaja.FirstOrDefault(c => c.UserId == nombreUsuario && c.Estado == EstadoCajas.Abierta);
        }
    }


}

