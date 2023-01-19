using Infrastructure.EntiesContext;
using Infrastructure.IdentityContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ConfigureContext
    {
        public static void Configure(IServiceCollection provider, IConfiguration configuration)
        {
   
            provider.AddDbContext<IdentityDbContext>(c => c.UseNpgsql(configuration.GetConnectionString("Identity")));
            provider.AddDbContext<EntitiesDbContext>(c => c.UseNpgsql(configuration.GetConnectionString("Entities")));

        }
    }
}
