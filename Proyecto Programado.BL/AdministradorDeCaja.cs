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

        public AdministradorDeCaja(DBContexto elContexto)
        {
            ElContexto = elContexto;
        }

        public void AbraLaCaja(Model.AperturaDeCaja laCaja, string elNombreDeUsuario)
        {
            laCaja.UserId = elNombreDeUsuario;
            laCaja.FechaDeInicio = DateTime.Now;
            laCaja.Estado = EstadoCajas.Abierta;
            //ESTO
            laCaja.Efectivo = 0;
            laCaja.Tarjeta = 0;
            laCaja.SinpeMovil = 0;

            ElContexto.AperturasDeCaja.Add(laCaja);
            ElContexto.SaveChanges();
        }
        public void CierreLaCaja(int elId)
        {
            var laCaja = ElContexto.AperturasDeCaja.FirstOrDefault(elElemento => elElemento.Id == elId);


            laCaja.FechaDeCierre = DateTime.Now;
            laCaja.Estado = EstadoCajas.Cerrada;


            ElContexto.SaveChanges();

        }






        public AperturaDeCaja ObtengaLaCajaAbierta(string elNombreDeUsuario)
        {

            return ElContexto.AperturasDeCaja.FirstOrDefault(elElemento => elElemento.UserId == elNombreDeUsuario && elElemento.Estado == EstadoCajas.Abierta);

        }
    }



}

