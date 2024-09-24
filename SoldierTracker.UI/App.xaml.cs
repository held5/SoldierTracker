
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoldierTracker.Application;
using SoldierTracker.Infrastructure;
using SoldierTracker.Infrastructure.Persistence;
using SoldierTracker.UI.Views;
using System.IO;
using System.Windows;

namespace SoldierTracker.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Configuration = CreateConfiguration();
            ServiceProvider = CreateServiceProvider();

            // Update migrations and seed default data
            await ServiceProvider.InitialiseDatabaseAsync();

            var monitor = ServiceProvider.GetRequiredService<SoldierMonitor>();
            monitor.Show();
        }

        private static IServiceProvider CreateServiceProvider()
        {
            return new ServiceCollection()
               .AddInfrastructureServices(Configuration)
               .AddApplicationServices(Configuration)
               .AddUIServices()
               .BuildServiceProvider();
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        }
    }
}
