using AuthenticationApi.Application.Interface;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using E_CommerceSharedLibrary.DependencyInjecation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApi.Infrastructure.Dependency
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext + logging (SharedServiceContainer)
            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(
                services,
                configuration,
                configuration["MySerilog:FileName"]!
            );

            // Dependency Injection
            services.AddScoped<IUserInterface, UserRepository>();

            return services;
        }

        public static IApplicationBuilder UseInterfacePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicy(app);
            return app;
        }
    }
}
