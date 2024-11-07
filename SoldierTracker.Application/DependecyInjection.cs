using Microsoft.Extensions.DependencyInjection;
using SoldierTracker.Application.Services;

namespace SoldierTracker.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ISoldierService, SoldierService>();

            return services;
        }
    }
}
