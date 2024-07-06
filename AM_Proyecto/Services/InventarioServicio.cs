using AM_Proyecto.Interfaces;
using AM_Proyecto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AM_Proyecto.Services
{
    public class InventarioServicio : IInventarioServicio
    {
        private readonly HttpClient httpClient;

        public InventarioServicio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Inventario>> GetAllInventarios()
        {
            var response = await httpClient.GetAsync("https://localhost:7237/ModuloCatalogoDeInventarios/ObtengaLaListaDeInventarios");
            if (response.IsSuccessStatusCode)
            {
                var inventories = await response.Content.ReadFromJsonAsync<List<Inventario>>();
                return inventories;
            }
            return default;
        }
    }
}
