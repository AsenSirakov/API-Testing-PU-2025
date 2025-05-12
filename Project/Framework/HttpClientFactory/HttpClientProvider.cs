using System.Net.Http.Headers;

namespace FinalProject.Framework.HttpClientFactory
{
    public class HttpClientProvider
    {
        public HttpClient Client { get; }

        public HttpClientProvider(TestConfiguration config)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(config.BaseUrl)
            };

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
        }
    }
}
