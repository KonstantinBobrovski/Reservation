using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityContext
{
    public class IdentityContext:DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> optionsBuilder) : base(optionsBuilder)
        { }

        DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
