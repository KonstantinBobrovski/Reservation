using Infrastructure.EntiesContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static void ConfigureInfastructue(IConfiguration configuration, IServiceCollection services)
        {
          
            services.AddDbContext<EntitiesContext>(c =>
                c.UseSqlServer("Server=sqlServer;Database=Reservation;User Id=SA;Password=AzMogaTukISega;"));
        }
    }
}