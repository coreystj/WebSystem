using WebSystem.MySQLASP.NET.Managers;
using WebSystem.MySQLASP.NET.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebSystem.MySQLASP.NET.Helpers
{
    public static class QueryHelper
    {
        public static string GenerateParameterName(Dictionary<string, object> arguments)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            foreach (string key in arguments.Keys)
            {
                result.Append(key + ((i < arguments.Count - 1) ? ", " : string.Empty));
                i++;
            }

            return result.ToString();
        }

        public static string GenerateParameterValues(Dictionary<string, object> arguments)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            foreach (object value in arguments.Values)
            {
                result.Append("'"+value+ "'" + ((i < arguments.Count - 1) ? ", " : string.Empty));
                i++;
            }

            return result.ToString();
        }

        public static string GenerateUpdateParameters(Dictionary<string, object> arguments)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            string value;
            foreach (string key in arguments.Keys)
            {
                value = arguments[key] as string;
                result.Append(key+"='" + value + "'" + 
                    ((i < arguments.Count - 1) ? ", " : string.Empty));
                i++;
            }

            return result.ToString();
        }

        public static string GetContent(this HttpRequestMessage request)
        {
            Task<string> result = request.Content.ReadAsStringAsync();
            return result.Result;
        }

        public static T Get<T>(this HttpRequestMessage request)
            where T : new()
        {
            string rawJson = request.GetContent();
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawJson);
            return json.Get<T>();
        }
    }
}