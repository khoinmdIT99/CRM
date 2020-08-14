using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM
{
   public class Event
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int CustID { get; set; }

        public int ContactID { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(1);

        public int TypeID { get; set; }

        public int ForUserID { get; set; }

        public DateTime? ForCareDate { get; set; }

        public string FileName { get; set; }

        public int StatusID { get; set; }

        public string Remarks { get; set; }

    }

    public enum Types
    {
        PhoneCall = 1,
        Meeting = 2
    }
}
