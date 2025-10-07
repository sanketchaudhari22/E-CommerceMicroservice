using E_CommerceSharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interface;
using OrderApi.Application.Service;
using Polly;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace OrderApi.Application.Dependency
{
    public static class ServiceContainer
    {
        // Extension method must be in a static class
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register HttpClient for OrderService
            services.AddHttpClient<IOrderService, OrderService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiGateway:BaseAddress"]!);
                client.Timeout = TimeSpan.FromSeconds(1);
            });

            // Create retry strategy using Polly
            var retryStrategy = Policy.Handle<TaskCanceledException>()
                                      .WaitAndRetryAsync(
                                          retryCount: 3,
                                          sleepDurationProvider: _ => TimeSpan.FromMilliseconds(500),
                                          onRetry: (exception, timeSpan, retryCount, context) =>
                                          {
                                              string message = $"Retry attempt {retryCount}, Exception: {exception.Message}";
                                              LogException.LogToConsole(message);
                                              LogException.LogToDebuger(message);
                                          });

            // Optional: you can register the retry policy in DI if needed
            services.AddSingleton(retryStrategy);

            return services;
        }
    }
}
