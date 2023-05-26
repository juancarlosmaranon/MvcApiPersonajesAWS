using MvcApiPersonajesAWS.Models;

namespace MvcApiPersonajesAWS.Services
{
    public class ServiceApiPersonajes
    {
        private string UrlApi;
        private string UrlApiEC2;
        private readonly IHttpClientFactory HttpClientFactory;

        public ServiceApiPersonajes(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiPersonajes");
            this.UrlApiEC2 = configuration.GetValue<string>("ApiUrls:ApiPersonajesEC2");
            HttpClientFactory = httpClientFactory;
        }

        private async Task<T?> CallApiAsync<T>(string urlApi, string request)
        {
            using HttpClient httpClient = HttpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(urlApi);
            var response = await httpClient.GetAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadAsAsync<T>();
        }

        private async Task<T?> CallApiNoSSLAsync<T>(string urlApi, string request)
        {

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            };

            var httpClient = new HttpClient(handler);

            httpClient.BaseAddress = new Uri(urlApi);
            var response = await httpClient.GetAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadAsAsync<T>();
        }

        public async Task<List<Personaje>?> GetPersonajesAsync()
        {
            return await this.CallApiAsync<List<Personaje>>(this.UrlApi, "/api/personajes");
        }

        public async Task<List<Personaje>?> GetPersonajesEC2Async()
        {
            return await this.CallApiNoSSLAsync<List<Personaje>>(this.UrlApiEC2, "/api/personajes");
        }
    }
}
