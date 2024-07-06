using AM_Proyecto.Interfaces;
using AM_Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AM_Proyecto.Services
{
    public class VentaServicio : IVentaServicio
    {
        private readonly HttpClient httpClient;

        public VentaServicio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Venta>> GetVentasDiarias()
        {
            var response = await httpClient.GetAsync("api/Ventas/ResumenDiario");
            if (response.IsSuccessStatusCode)
            {
                var ventas = await response.Content.ReadFromJsonAsync<List<Venta>>();
                return ventas;
            }
            return new List<Venta>();
        }
    }
}
