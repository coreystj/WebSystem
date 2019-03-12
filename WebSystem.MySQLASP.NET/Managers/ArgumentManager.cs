using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WebSystem.MySQLASP.NET.Managers
{
    public static class ArgumentManager
    {
        public static Dictionary<string, object> GetArguments(this HttpRequestMessage request)
        {
            var arguments = new Dictionary<string, object>();

            foreach (var argument in request.Headers)
                arguments.Add(argument.Key, argument.Value.FirstOrDefault());

            foreach (var argument in request.GetQueryNameValuePairs())
                arguments.Add(argument.Key, argument.Value);

            return arguments;
        }

        public static T Get<T>(this HttpRequestMessage request, string key)
        {
            var arguments = request.GetArguments();
            if (arguments.TryGetValue(key, out object value))
            {
                if (value is T)
                    return (T)value;
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
            else
                throw new KeyNotFoundException("Key \""+ key +"\" does not exist.");
        }
    }
}