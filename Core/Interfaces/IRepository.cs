using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
namespace Core.Interfaces
{
    public interface IRepository<T>: IRepositoryBase<T>, IReadRepository<T> where T : class
    {

    }
}
