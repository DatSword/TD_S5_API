using TD1BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TD1BlazorApp.Services;

namespace TD1BlazorApp.Services
{
    public class WSService<T> : IService<T>
    {
        private readonly HttpClient httpClient;
        private readonly string endpoint;

        public WSService(HttpClient httpClient, string endpoint)
        {
            this.httpClient = httpClient;
            this.endpoint = endpoint;
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<T>> GetItemsAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<T>>(endpoint);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
