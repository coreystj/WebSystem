using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSystem.MySQLASP.NET.Helpers
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// Retrieves a value from a dictionary.
        /// </summary>
        /// <typeparam name="T">The type of value to be retrieved.</typeparam>
        /// <param name="dict">The dictionary to retrieve from.</param>
        /// <param name="key">The key to be found.</param>
        /// <returns>Returns the found value.</returns>
        public static T Get<T>(this Dictionary<string, object> dict, string key)
        {
            object value;
            if (dict.TryGetValue(key, out value))
            {
                if (value is T)
                {
                    return (T)value;
                }
                try
                {
                    if (value == null && typeof(T) == typeof(string))
                        return (T)Convert.ChangeType(string.Empty, typeof(T));
                    else
                        return default(T);
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
            else
                throw new Exception(string.Format("Key '{0}' does not exist.", key));
        }

        /// <summary>
        /// Safely retrieves an element from a dictionary.
        /// </summary>
        /// <typeparam name="T">The type of value to be retrieved.</typeparam>
        /// <param name="dict">The dictionary to retrieve a value from.</param>
        /// <param name="key">The key to be filtered.</param>
        /// <param name="defaultValue">The default value used if not found.</param>
        /// <returns>Returns the found value.</returns>
        public static T SafeGet<T>(this Dictionary<string, object> dict, string key, T defaultValue = default(T))
        {
            try
            {
                return dict.Get<T>(key);
            }
            catch (Exception)
            {
                if (typeof(T) == typeof(string))
                    return (T)Convert.ChangeType(string.Empty, typeof(T));
                else
                    return defaultValue;
            }
        }
    }
}
