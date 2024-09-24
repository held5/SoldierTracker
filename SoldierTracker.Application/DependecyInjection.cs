using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoldierTracker.Application.Models;
using SoldierTracker.Application.Services;

namespace SoldierTracker.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SensorHubSettings>(configuration.GetSection("SensorHub"));
            services.AddSingleton<IHubConnectionFactory, HubConnectionFactory>();
            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddScoped<ISoldierService, SoldierService>();

            return services;
        }
    }
}
