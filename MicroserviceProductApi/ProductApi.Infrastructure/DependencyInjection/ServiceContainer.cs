using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ProductApi.Application.Interface;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using E_CommerceSharedLibrary.DependencyInjecation;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext + Serilog (Shared services)
            SharedServiceContainer.AddSharedServices<ProductDbContext>(
                services,
                configuration,
                configuration["MySerilog:FileName"] ?? string.Empty
            );

            // Register repositories
            services.AddScoped<IProductInterface, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicy(app);
            return app;
        }
    }
}
