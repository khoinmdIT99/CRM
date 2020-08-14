using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface ICustRepository
    {
        Task<IEnumerable<Cust>> Get();
        Task<Cust> GetByID(int id);
        Task<Cust> GetFull(int id);
        Task<Cust> Save(Cust item);

        Task<Cust> Add (Cust item);
        void Delete(int id);
        Task<Cust> Update(Cust item);
    }
}
