using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SmileChef.Repository;

namespace ChefAppTestProject
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing IChefRepository registration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IChefRepository));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a new IChefRepository registration with the concrete implementation
                services.AddScoped<IChefRepository, ChefRepository>();

                // You can optionally configure other services as needed for testing
                // For example, add mocks or configure the database context for testing
            });
        }
    }
}
