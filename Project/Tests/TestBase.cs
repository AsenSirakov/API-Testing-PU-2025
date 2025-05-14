using FinalProject.Framework.HttpClientFactory;
using FinalProject.Framework.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.Tests
{
    public class TestBase
    {
        protected HttpClient Client;
        protected UserService UserService;
        private ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: false)
                .Build();

            var services = new ServiceCollection();
            services.RegisterServices(configuration);
            _serviceProvider = services.BuildServiceProvider();

            var httpClientProvider = _serviceProvider.GetRequiredService<HttpClientProvider>();
            Client = httpClientProvider.Client;
            UserService = _serviceProvider.GetRequiredService<UserService>();
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            if (_serviceProvider != null)
            {
                _serviceProvider.Dispose();
            }
        }
    }
}