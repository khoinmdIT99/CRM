using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class PageModel
    {
        public object Item { get; set; } = new object();

        public object Temp { get; set; } = new object();

        //public List<SelectItem> SelectItems { get; set; } = new List<SelectItem>();

        public Dictionary<string, List<SelectItem>> Selects { get; set; } = new Dictionary<string, List<SelectItem>>();

    }
    public class SelectItem
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public object Extra { get; set; }

    }
}