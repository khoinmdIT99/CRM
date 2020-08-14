using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM
{
    [Table("Users")]
   public class User
    {
        public int ID { get; set; }

        public Guid UID { get; set; } = Guid.NewGuid();

        public string CompanyCode { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
