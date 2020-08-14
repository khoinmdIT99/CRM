using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM
{
   public class Contact
    {
        public int ID { get; set; }

        public int CustID { get; set; }

        public string Name { get; set; }

        public int PositionID { get; set; }

        public string Email { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int StatusID { get; set; }

        public string Remarks { get; set; }

    }
}
