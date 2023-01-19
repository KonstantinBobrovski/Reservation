using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityContext
{

    /// <summary>
    /// Used For Migrations
    /// </summary>
    internal class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory() , @"..\Reservation"));

            var configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json")
            .Build();


            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Identity"));

            return new IdentityDbContext(optionsBuilder.Options);
            
        }
    }
}
