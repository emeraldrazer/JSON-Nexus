using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSON_Nexus.Functions
{
    public static class CreateJSON
    {
        public static string Create(Dictionary<string, object> properties)
        {
            return JsonConvert.SerializeObject(properties);
        }

        public static string Create(IEnumerable<string> properties)
        {
            return JsonConvert.SerializeObject(properties);
        }

        public static string Create(string properties)
        {
            return JsonConvert.SerializeObject(properties);
        }
    }
}
