using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeVenta : IAdministradorDeVentas
    {
        public DBContexto ElContexto;

        public AdministradorDeVenta(DBContexto contexto)
        {
            ElContexto = contexto;
        }
        public List<Inventario> ObtenLaListaDeInventarios()
        {

            var laListaDeInventarios = ElContexto.Inventarios.ToList();

            return laListaDeInventarios;
        }

        public void RegistreVenta(Model.Venta venta)
        {
            try
            {
                // Verificar si el usuario tiene una apertura de caja válida
                var aperturaDeCaja = ElContexto.AperturasDeCaja.FirstOrDefault(a => a.Id == venta.IdAperturaDeCaja && a.Estado == EstadoCajas.CajaAbierta);
                if (aperturaDeCaja == null)
                {
                    throw new InvalidOperationException("No tiene una apertura de caja abierta.");
                }

                // Asignar la apertura de caja y otros valores a la venta
                venta.Fecha = DateTime.Now;
                venta.Estado = EstadoVenta.EnProceso;
                venta.IdAperturaDeCaja = aperturaDeCaja.Id;

                ElContexto.Ventas.Add(venta);
                ElContexto.SaveChanges();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones aquí, puedes loguear o lanzar la excepción nuevamente si es necesario
                throw new Exception("Error al registrar la venta.", ex);
            }
        }


    }
}
