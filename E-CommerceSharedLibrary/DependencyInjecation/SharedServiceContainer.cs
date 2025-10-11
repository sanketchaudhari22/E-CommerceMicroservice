using E_CommerceSharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace E_CommerceSharedLibrary.DependencyInjecation
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string filename) where TContext : DbContext
        {
            // 🔹 Add generic database context with retry
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("E-CommerceMicroservice"),
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
                ));

            // 🔹 Configure Serilog logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                    path: $"{filename}-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    shared: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            // 🔹 Add Serilog to DI
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(dispose: true);
            });

            // 🔹 Add JWT auth if you already implemented JwtAuthenticationSchem class

            // JwtAuthenticationSchem.AddJwtAutheticationschem(services, configuration);

            return services;
        }

        // 🔹 Extension for using middleware
        public static IApplicationBuilder UseSharedPolicy(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
