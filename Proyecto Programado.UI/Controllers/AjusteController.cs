using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_Programado.UI.Controllers
{
    [Authorize]
    public class AjusteController : Controller
    {
        public readonly IAdministradorDeAjustes ElAdministrador;

        public AjusteController(IAdministradorDeAjustes administrador)
        {
            ElAdministrador = administrador;
        }

        // GET: AjusteController
        public ActionResult Index(int id, string nombre)
        {
            List<Inventario> laListaDeInventarios;
            laListaDeInventarios = ElAdministrador.ObtengaLaListaDeInventarios();


            if (nombre is null)
            {
                return View(laListaDeInventarios);
            }
            else
            {
                List<Inventario> laListadeInventariosFiltrada;
                laListadeInventariosFiltrada = laListaDeInventarios.Where(x => x.Nombre.Contains(nombre)).ToList();
                return View(laListadeInventariosFiltrada);

            }
            
        }


       
        // GET: AjusteController
        public ActionResult MostrarAjustes(int id)
        {
            List<AjusteDeInventario> laListaDeAjustes;
            laListaDeAjustes = ElAdministrador.ObtengaLosAjustes(id);

            return View(laListaDeAjustes);
        }




        // GET: AjusteController/Details/5
        public ActionResult Details(int id)
        {

            Model.AjusteDeInventario ajustesInventario;
            ajustesInventario = ElAdministrador.ObtengaLosAjustesDeInventario(id);

            return View(ajustesInventario);
        
        }


        // GET: AjusteController/Create
        public ActionResult AgregarAjuste(int id_inventario)
        {
            var cantidad = ElAdministrador.ObtengaLaCantidadActual(id_inventario);

            Model.AjusteDeInventario cantidadActual = new Model.AjusteDeInventario
            {
                CantidadActual = cantidad,
                Id_Inventario = id_inventario,
                Ajuste = 0
            };

            return View(cantidadActual);
        }

        // POST: AjusteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarAjuste(AjusteDeInventario nuevoAjuste)
        {
            try
            {

                ElAdministrador.AgregueUnAjuste(nuevoAjuste, User.Identity.Name);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AjusteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AjusteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AjusteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AjusteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
