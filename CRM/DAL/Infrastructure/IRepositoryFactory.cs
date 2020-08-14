using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface IRepositoryFactory<T>
    {
        //IDbConnection GetConnection();
        Task<IEnumerable<T>> Get();
        Task<T> GetByID(int id);
        Task Add(T item);
        Task Delete(int id);
        Task Update(T item);
    }
}
