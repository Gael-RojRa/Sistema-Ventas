using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto_Programado.BL;
using Proyecto_Programado.Model;
using Proyecto_Programado.UI.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;

namespace Proyecto_Programado.UI.Controllers
{
    [Authorize]
    public class CajaController : Controller
    {
        private readonly IAdministradorDeCaja ElAdministrador;

        public CajaController(IAdministradorDeCaja administrador)
        {
            ElAdministrador = administrador;
        }

        // GET: CajaController
        public async Task<ActionResult> Index()
        {
            string elNombreDeUsuario = User.Identity.Name;
            var httpclient = new HttpClient();
            var url = $"https://localhost:7237/ModuloDeCajas/ObtengaLaCajaAbierta?elNombreDeUsuario={elNombreDeUsuario}";
            var respuesta = await httpclient.GetAsync(url);
            AperturaDeCaja cajaAbierta = JsonConvert.DeserializeObject<AperturaDeCaja>(await respuesta.Content.ReadAsStringAsync());
            return View(cajaAbierta);
        }



        // GET: CajaController/Create
        public async Task<ActionResult> Create(AperturaDeCaja caja)
        {
            var httpClient = new HttpClient();
            string elNombreDeUsuario = Uri.EscapeDataString(User.Identity.Name);

            var json = JsonConvert.SerializeObject(caja, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://localhost:7237/ModuloDeCajas/AbraLaCaja?elNombreDeUsuario={elNombreDeUsuario}";
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var respuesta = await httpClient.PostAsync(url, content);
            var respuestaContenido = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception($"Error {respuesta.StatusCode}: {respuestaContenido}");
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: CajaController
        public ActionResult CerrarCaja(int id)
        {
            var httpClient = new HttpClient();
            var url = $"https://localhost:7237/ModuloDeCajas/CierreLaCaja/{id}";
            var respuesta = httpClient.PutAsync(url, null).Result;
            return RedirectToAction(nameof(Index));

        }

    }
}

