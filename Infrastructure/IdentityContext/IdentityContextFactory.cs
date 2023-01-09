using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            optionsBuilder.UseSqlServer("Server=sqlServer;Database=Identity;User=sa;Password=S3cur3P@ssW0rd!;TrustServerCertificate=true;");

            return new IdentityDbContext(optionsBuilder.Options);
            
        }
    }
}
