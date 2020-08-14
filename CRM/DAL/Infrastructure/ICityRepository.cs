using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface ICityRepository
    {
        Task<IEnumerable<City>> Get();
        Task<City> GetByID(int id);
    }
}
