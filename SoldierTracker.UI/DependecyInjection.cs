using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoldierTracker.Application.Models;
using SoldierTracker.UI.Utility;
using SoldierTracker.UI.ViewModels;
using SoldierTracker.UI.Views;

namespace SoldierTracker.UI
{
    internal static class DependecyInjection
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole());
            services.Configure<SensorHubSettings>(App.Configuration.GetSection("SensorHub"));
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient<SoldierMonitor>();
            services.AddTransient<SoldierMonitorViewModel>();

            return services;
        }
    }
}
