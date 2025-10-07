using E_CommerceSharedLibrary.DependencyInjecation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interface;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories; // Make sure OrderRepository namespace is correct

namespace OrderApi.Infrastructure.Dependency
{
    public static class InfrastructureServiceExtensions
    {
        // Extension method to add infrastructure services
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add database connectivity (replace OrderDbContext with your actual DbContext)
            SharedServiceContainer.AddSharedServices<OrderDbContext>(
                services,
                configuration,
                configuration["MySerialLog:FileName"]!);

            // Register DI
            services.AddScoped<IOrderInterface, OrderRepository>();

            return services;
        }

        // Extension method to configure middleware/policies
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Example: register global exception handler, API gateway policies, etc.
            SharedServiceContainer.UseSharedPolicy(app);

            return app;
        }
    }
}
