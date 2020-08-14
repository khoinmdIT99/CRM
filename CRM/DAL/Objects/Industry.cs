using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM
{
   public class Industry
    {
        public int ID { get; set; }

        public int MainIndustryID { get; set; }

        public string Name { get; set; }
    }
}
