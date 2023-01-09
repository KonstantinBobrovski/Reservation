using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntiesContext
{
   

    /// <summary>
    /// Used For Migrations
    /// </summary>
    internal class EntititesContextFactory : IDesignTimeDbContextFactory<EntitiesDbContext>
    {
        public EntitiesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EntitiesDbContext>();
            optionsBuilder.UseSqlServer("Server=sqlServer;Database=Application;User=sa;Password=S3cur3P@ssW0rd!;TrustServerCertificate=true;");

            return new EntitiesDbContext(optionsBuilder.Options);

        }
    }
}
