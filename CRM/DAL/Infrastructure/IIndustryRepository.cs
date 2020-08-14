using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface IIndustryRepository
    {
        Task<IEnumerable<Industry>> Get();
        Task<Industry> GetByID(int id);
       
    }
}
