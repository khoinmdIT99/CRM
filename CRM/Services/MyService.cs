using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class MyService: IMyService {
        public User User { get; set; } 
        public Guid UserUid { get; set; }

    }
}
