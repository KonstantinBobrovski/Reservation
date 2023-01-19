using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.EntiesContext
{
   

    /// <summary>
    /// Used For Migrations
    /// </summary>
    internal class EntititesContextFactory : IDesignTimeDbContextFactory<EntitiesDbContext>
    {
        public EntitiesDbContext CreateDbContext(string[] args)
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\Reservation"));

            var configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json")
           .Build();



            var optionsBuilder = new DbContextOptionsBuilder<EntitiesDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Entities"));

            return new EntitiesDbContext(optionsBuilder.Options);

        }
    }
}
