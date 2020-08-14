using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CommonData
{
   public class UsersRepository : RepositoryFactory<User>
    {
        public UsersRepository() : base("Users") { }

        
    }
}
