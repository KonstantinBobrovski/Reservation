using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Core.Interfaces;
using Reservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntiesContext
{
    public class EfCoreRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
    {
     
        public EfCoreRepository(EntitiesDbContext entitiesDbContext):base(entitiesDbContext) {
          
        }

        
    }
}
