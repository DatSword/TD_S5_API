using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


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

        public async Task<HttpResponseMessage> PostItemAsync(T item)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, item);

                return response;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
