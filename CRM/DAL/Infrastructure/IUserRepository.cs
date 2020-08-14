using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
   public interface IUserRepository
    {
        Task<IEnumerable<User>> Get();
        Task<User> GetByID(int id);
        Task<User> Login(string username, string password);

         Task<User> Add (User item);
        void Delete(int id);
        Task<User> Update(User item);
    }
}
