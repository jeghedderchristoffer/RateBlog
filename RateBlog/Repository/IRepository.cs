using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Repository
{
    public interface IRepository <T> where T : BaseEntity
    {
        T Get(string id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity); 
    }
}
