using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Infrastructure.Utilities
{
    public static class WebAPIUtility
    {
        public static JsonSerializerSettings JsonSerializerSettings => new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public static string ToJson(this object netObject)
        {
            return JsonConvert.SerializeObject(netObject, JsonSerializerSettings);
        }
    }
}
