using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Helpers
{
    public static class DictionaryHelper
    {
        public static T Get<T>(this Dictionary<string, object> dict, string key)
        {
            object value;
            if (dict.ContainsKey(key))
            {
                value = dict[key];
                if (value is T)
                {
                    return (T)value;
                }
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
                throw new Exception(string.Format("Key '{0}' does not exist.", key));
        }

        public static T SafeGet<T>(this Dictionary<string, object> dict, string key, T value = default(T))
        {
            try
            {
                return dict.Get<T>(key);
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}
