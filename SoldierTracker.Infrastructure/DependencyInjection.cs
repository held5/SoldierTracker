using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoldierTracker.Domain.Interfaces;
using SoldierTracker.Infrastructure.Persistence;
using SoldierTracker.Infrastructure.Persistence.Interceptors;
using SoldierTracker.Infrastructure.Repositories;

namespace SoldierTracker.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SoldierTrackerDbContext>((c, options) =>
            {
                options.AddInterceptors(c.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<SoldierTrackerDbContextDataInit>();
            services.AddScoped<ISaveChangesInterceptor, EntityInterceptor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton(TimeProvider.System);

            return services;
        }
    }
}
