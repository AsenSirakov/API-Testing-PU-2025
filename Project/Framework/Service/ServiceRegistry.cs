using FinalProject.Framework.HttpClientFactory;
using FinalProject.Framework.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.Framework.Service
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var testConfig = configuration.GetSection("TestConfiguration").Get<TestConfiguration>();
            services.AddSingleton(testConfig);
            services.AddSingleton(provider => new HttpClientProvider(testConfig));
            services.AddTransient<UserService>(provider =>
            {
                var httpClientProvider = provider.GetRequiredService<HttpClientProvider>();
                return new UserService(httpClientProvider.Client);
            });

            return services;
        }
    }
}