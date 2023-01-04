using Microsoft.EntityFrameworkCore;
using Reservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntiesContext
{
    public  class EntitiesContext:DbContext
    {
        public EntitiesContext(DbContextOptions<EntitiesContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
      
        public DbSet<Table> Tables { get; set; }
      
        public DbSet<Reservation.Core.Models.Reservation> Reservations { get; set; }

 
    }
}
