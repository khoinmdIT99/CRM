using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface IPositionRepository
    {
        Task<IEnumerable<Position>> Get();
        Task<Position> GetByID(int id);
       
    }
}
