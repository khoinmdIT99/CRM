using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface IMainIndustryRepository
    {
        Task<IEnumerable<MainIndustry>> Get();
        Task<MainIndustry> GetByID(int id);
       
    }
}
