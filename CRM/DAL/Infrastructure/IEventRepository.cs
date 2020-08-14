using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface IEventRepository
    {
        Task<IEnumerable<Event>> Get();
        Task<Event> GetByID(int id);
         Task<Event> Add (Event item);
        Task<Event> Save(Event item);

        void Delete(int id);
        Task<Event> Update(Event item);
    }
}
