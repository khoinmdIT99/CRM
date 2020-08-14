using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM
{
   public class Cust
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int CityID { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int MainIndustryID { get; set; }

        public int Industry { get; set; }

        public int StatusID { get; set; }

        public string Description { get; set; }

        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public List<Event> Events { get; set; } = new List<Event>();


    }
}
