using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface IContactRepository
    {
        Task<IEnumerable<Contact>> Get();
        Task<Contact> GetByID(int id);
        Task<IEnumerable<Contact>> GetByCustID(int custID);
         Task<Contact> Add (Contact item);
        Task<Contact> Save(Contact item);

        void Delete(int id);
        Task<Contact> Update(Contact item);
    }
}
