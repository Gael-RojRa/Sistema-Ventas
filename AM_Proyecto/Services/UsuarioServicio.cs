using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AM_Proyecto.Interfaces;

namespace AM_Proyecto.Services
{
    public class UserService : IUsuarioServicio
    {
        string urlApi = "https://webappjunio2024.azurewebsites.net/api/user/";
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AuthenticateUserAsync(string nombre, string clave)
        {
            var usuario = new { Nombre = nombre, Clave = clave };
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(urlApi, content);
            return response.IsSuccessStatusCode;
        }
    }
}
