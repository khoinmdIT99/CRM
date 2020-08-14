using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRM.Extantions
{
    public static class JsonHelper
    {
        public static string ToJson(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json;

        }
    }
}
