using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace WebSystem.MySQLUnity.Helpers
{
    public static class ObjectHelper
    {
        public static Dictionary<string, object> Serialize<T>(this T item)
            where T: new()
        {
            string[] columns = GetPropertyNames<T>(item, omitId: false);
            object[] values = GetPropertyValues<T>(item, omitId: false);
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < columns.Length; i++)
            {
                dict.Add(columns[i], values[i]);
            }
            return dict;
        }

        public static string GetPreferredName(PropertyInfo property)
        {
            XmlElementAttribute preferredAttribute = property.GetCustomAttributes(typeof(XmlElementAttribute), false).Cast<XmlElementAttribute>()
                .FirstOrDefault();
            return (preferredAttribute == null) ? property.Name : preferredAttribute.ElementName;
        }

        public static string[] GetPropertyNames<T>(T item, bool omitId = true)
            where T : new()
        {

            PropertyInfo[] properties = item.GetType().GetProperties();
            if (omitId)
                properties = properties.Where(x => x.Name != "Id").ToArray();
            string[] names = new string[properties.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = GetPreferredName(properties[i]);
            }

            return names;
        }

        public static object[] GetPropertyValues<T>(T item, bool omitId = true)
            where T : new()
        {

            PropertyInfo[] properties = item.GetType().GetProperties();
            if (omitId)
                properties = properties.Where(x => x.Name != "Id").ToArray();
            object[] names = new object[properties.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = properties[i].GetValue(item, new object[0]).ToString();
            }

            return names;
        }

        public static string GetPreferredName(Type obj)
        {
            XmlElementAttribute preferredAttribute = obj.GetCustomAttributes(typeof(XmlElementAttribute), false).Cast<XmlElementAttribute>()
                .FirstOrDefault();
            return (preferredAttribute == null) ? obj.Name : preferredAttribute.ElementName;
        }


        public static T[] GetValues<T>(this Dictionary<string, object> dict, string key)
            where T : new()
        {
            var modelsJson = dict.Get<List<object>>(key);
            return modelsJson.Get<T>();
        }

        public static T[] Get<T>(this List<object> modelsJson)
            where T : new()
        {

            T[] tmp = new T[modelsJson.Count];
            int i = 0;
            foreach (Dictionary<string, object> modelJson in modelsJson)
            {
                tmp[i++] = modelJson.Get<T>();
            }

            return tmp;
        }

        public static T Get<T>(this Dictionary<string, object> dict)
            where T : new()
        {

            var tmp = new T();

            foreach (string key in dict.Keys)
            {
                PropertyInfo[] properties = tmp.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (key == GetPreferredName(property) || property.Name == key)
                    {
                        if (key.ToUpper() == "ID")
                        {
                            if (dict[key] is long)
                                property.SetValue(tmp, (int)((long)dict[key]), new object[0]);
                            else if (dict[key] is int)
                                property.SetValue(tmp, ((int)dict[key]), new object[0]);
                            else if (dict[key] is string)
                                property.SetValue(tmp, int.Parse(dict[key] as string), new object[0]);
                        }
                        else
                            property.SetValue(tmp, dict[key], new object[0]);
                        break;
                    }
                }
            }

            return tmp;
        }
    }
}
