
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using WebSystem.MySQLUnity.Helpers;

namespace WebSystem.MySQLUnity.IO
{
    /// <summary>
    /// This static class parses raw json strings.
    /// </summary>
    public static class JsonConvert
    {
        [ThreadStatic]
        static Stack<List<string>> splitArrayPool;
        [ThreadStatic]
        static StringBuilder stringBuilder;
        [ThreadStatic]
        static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache;
        [ThreadStatic]
        static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache;

        /// <summary>
        /// Retrieves a Dictionary object from a raw json.
        /// </summary>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        /// <param name="json">The raw json.</param>
        /// <returns>Returns the newly created json object.</returns>
        public static T ToObject<T>(this string json)
        {
            // Initialize, if needed, the ThreadStatic variables
            if (null == propertyInfoCache) propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
            if (null == fieldInfoCache) fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
            if (null == stringBuilder) stringBuilder = new StringBuilder();
            if (null == splitArrayPool) splitArrayPool = new Stack<List<string>>();

            //Remove all whitespace not within strings to make parsing simpler
            stringBuilder.Length = 0;
            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"')
                {
                    i = AppendUntilStringEnd(true, i, json);
                    continue;
                }
                if (char.IsWhiteSpace(c))
                    continue;

                stringBuilder.Append(c);
            }

            //Parse the thing!
            return (T)ParseValue(typeof(T), stringBuilder.ToString());
        }

        private static int AppendUntilStringEnd(bool appendEscapeCharacter, int startIdx, string json)
        {
            stringBuilder.Append(json[startIdx]);
            for (int i = startIdx + 1; i < json.Length; i++)
            {
                if (json[i] == '\\')
                {
                    if (appendEscapeCharacter)
                        stringBuilder.Append(json[i]);
                    stringBuilder.Append(json[i + 1]);
                    i++;//Skip next character as it is escaped
                }
                else if (json[i] == '"')
                {
                    stringBuilder.Append(json[i]);
                    return i;
                }
                else
                    stringBuilder.Append(json[i]);
            }
            return json.Length - 1;
        }

        //Splits { <value>:<value>, <value>:<value> } and [ <value>, <value> ] into a list of <value> strings
        private static List<string> Split(string json)
        {
            List<string> splitArray = splitArrayPool.Count > 0 ? splitArrayPool.Pop() : new List<string>();
            splitArray.Clear();
            if (json.Length == 2)
                return splitArray;
            int parseDepth = 0;
            stringBuilder.Length = 0;
            for (int i = 1; i < json.Length - 1; i++)
            {
                switch (json[i])
                {
                    case '[':
                    case '{':
                        parseDepth++;
                        break;
                    case ']':
                    case '}':
                        parseDepth--;
                        break;
                    case '"':
                        i = AppendUntilStringEnd(true, i, json);
                        continue;
                    case ',':
                    case ':':
                        if (parseDepth == 0)
                        {
                            splitArray.Add(stringBuilder.ToString());
                            stringBuilder.Length = 0;
                            continue;
                        }
                        break;
                }

                stringBuilder.Append(json[i]);
            }

            splitArray.Add(stringBuilder.ToString());

            return splitArray;
        }

        private static object ParseValue(Type type, string json)
        {
            if (type == typeof(string))
            {
                return ParseString(json);
            }
            if (type.IsEnum)
            {
                return EnumHelper.ParseEnum(ParseString(json), type);
            }
            if (type == typeof(UnityEngine.Vector2))
            {
                Dictionary<string, string> dict = Parse(json);
                return new UnityEngine.Vector2(float.Parse(dict["X"]), float.Parse(dict["Y"]));
            }
            if (type == typeof(UnityEngine.Vector3))
            {
                Dictionary<string, string> dict = Parse(json);
                return new UnityEngine.Vector3(float.Parse(dict["X"]), float.Parse(dict["Y"]), 
                    float.Parse(dict["Z"]));
            }
            if (type == typeof(UnityEngine.Quaternion))
            {
                Dictionary<string, string> dict = Parse(json);
                return new UnityEngine.Quaternion(float.Parse(dict["X"]), float.Parse(dict["Y"]), 
                    float.Parse(dict["Z"]), float.Parse(dict["W"]));
            }
            if (type == typeof(UnityEngine.Color))
            {
                Dictionary<string, string> dict = Parse(json);
                return new UnityEngine.Color(float.Parse(dict["R"]), float.Parse(dict["G"]), 
                    float.Parse(dict["B"]), float.Parse(dict["A"]));
            }
            if (type == typeof(Guid))
            {
                return new Guid(ParseString(json));
            }
            if (type == typeof(int))
            {
                int result;
                int.TryParse(json, out result);
                return result;
            }
            if (type == typeof(byte))
            {
                byte result;
                byte.TryParse(json, out result);
                return result;
            }
            if (type == typeof(float))
            {
                float result;
                float.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
                return result;
            }
            if (type == typeof(double))
            {
                double result;
                double.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
                return result;
            }
            if (type == typeof(bool))
            {
                return json.ToLower() == "true";
            }
            if (json == "null")
            {
                return null;
            }
            if (type.IsArray)
            {
                Type arrayType = type.GetElementType();
                if (json[0] != '[' || json[json.Length - 1] != ']')
                    return null;

                List<string> elems = Split(json);
                Array newArray = Array.CreateInstance(arrayType, elems.Count);
                for (int i = 0; i < elems.Count; i++)
                    newArray.SetValue(ParseValue(arrayType, elems[i]), i);
                splitArrayPool.Push(elems);
                return newArray;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type listType = type.GetGenericArguments()[0];
                if (json[0] != '[' || json[json.Length - 1] != ']')
                    return null;

                List<string> elems = Split(json);
                var list = (IList)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count });
                for (int i = 0; i < elems.Count; i++)
                    list.Add(ParseValue(listType, elems[i]));
                splitArrayPool.Push(elems);
                return list;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type keyType, valueType;
                {
                    Type[] args = type.GetGenericArguments();
                    keyType = args[0];
                    valueType = args[1];
                }

                //Refuse to parse dictionary keys that aren't of type string
                if (keyType != typeof(string))
                    return null;
                //Must be a valid dictionary element
                if (json[0] != '{' || json[json.Length - 1] != '}')
                    return null;
                //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
                List<string> elems = Split(json);
                if (elems.Count % 2 != 0)
                    return null;

                var dictionary = (IDictionary)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count / 2 });
                for (int i = 0; i < elems.Count; i += 2)
                {
                    if (elems[i].Length <= 2)
                        continue;
                    string keyValue = elems[i].Substring(1, elems[i].Length - 2);
                    object val = ParseValue(valueType, elems[i + 1]);
                    dictionary.Add(keyValue, val);
                }
                return dictionary;
            }
            if (type == typeof(object))
            {
                return ParseAnonymousValue(json);
            }
            if (json[0] == '{' && json[json.Length - 1] == '}')
            {
                return ParseObject(type, json);
            }

            return null;
        }

        private static Dictionary<string, string> Parse(string rawJson)
        {
            var dict = new Dictionary<string, string>();
            List<string> elems = Split(rawJson);

            for (int i = 0; i < elems.Count; i += 2)
            {
                if (elems[i].Length <= 2)
                    continue;
                string key = elems[i].Substring(1, elems[i].Length - 2);
                string value = elems[i + 1];
                dict[key] = value;
            }
            return dict;
        }

        private static string ParseString(string json)
        {
            if (json.Length <= 2)
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i < json.Length - 1; ++i)
            {
                if (json[i] == '\\' && i + 1 < json.Length - 1)
                {
                    int j = "\"\\nrtbf/".IndexOf(json[i + 1]);
                    if (j >= 0)
                    {
                        stringBuilder.Append("\"\\\n\r\t\b\f/"[j]);
                        ++i;
                        continue;
                    }
                    if (json[i + 1] == 'u' && i + 5 < json.Length - 1)
                    {
                        UInt32 c = 0;
                        if (UInt32.TryParse(json.Substring(i + 2, 4), System.Globalization.NumberStyles.AllowHexSpecifier, null, out c))
                        {
                            stringBuilder.Append((char)c);
                            i += 5;
                            continue;
                        }
                    }
                }
                stringBuilder.Append(json[i]);
            }
            return stringBuilder.ToString();
        }

        private static object ParseAnonymousValue(string json)
        {
            if (json.Length == 0)
                return null;
            if (json[0] == '{' && json[json.Length - 1] == '}')
            {
                List<string> elems = Split(json);
                if (elems.Count % 2 != 0)
                    return null;
                var dict = new Dictionary<string, object>(elems.Count / 2);
                for (int i = 0; i < elems.Count; i += 2)
                    dict.Add(elems[i].Substring(1, elems[i].Length - 2), ParseAnonymousValue(elems[i + 1]));
                return dict;
            }
            if (json[0] == '[' && json[json.Length - 1] == ']')
            {
                List<string> items = Split(json);
                var finalList = new List<object>(items.Count);
                for (int i = 0; i < items.Count; i++)
                    finalList.Add(ParseAnonymousValue(items[i]));
                return finalList;
            }
            if (json[0] == '"' && json[json.Length - 1] == '"')
            {
                string str = json.Substring(1, json.Length - 2);
                return str.Replace("\\", string.Empty);
            }
            if (char.IsDigit(json[0]) || json[0] == '-')
            {
                if (json.Contains("."))
                {
                    double result;
                    double.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
                    return result;
                }
                else
                {
                    int result;
                    int.TryParse(json, out result);
                    return result;
                }
            }
            if (json == "true")
                return true;
            if (json == "false")
                return false;
            // handles json == "null" as well as invalid JSON
            return null;
        }

        private static object ParseObject(Type type, string json)
        {
            object instance = FormatterServices.GetUninitializedObject(type);
            type = instance.GetType();
            //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
            List<string> elems = Split(json);
            if (elems.Count % 2 != 0)
                return instance;

            Dictionary<string, FieldInfo> nameToField;
            Dictionary<string, PropertyInfo> nameToProperty;
            if (!fieldInfoCache.TryGetValue(type, out nameToField))
            {
                nameToField = type.GetFields().Where(field => field.IsPublic).ToDictionary(field => field.Name);
                fieldInfoCache.Add(type, nameToField);
            }
            if (!propertyInfoCache.TryGetValue(type, out nameToProperty))
            {
                nameToProperty = type.GetProperties().ToDictionary(p => p.Name);
                propertyInfoCache.Add(type, nameToProperty);
            }

            for (int i = 0; i < elems.Count; i += 2)
            {
                if (elems[i].Length <= 2)
                    continue;
                string key = elems[i].Substring(1, elems[i].Length - 2);
                string value = elems[i + 1];
                try
                {
                    FieldInfo fieldInfo;
                    PropertyInfo propertyInfo;
                    if (nameToField.TryGetValue(key, out fieldInfo))
                        fieldInfo.SetValue(instance, ParseValue(fieldInfo.FieldType, value));
                    else if (nameToProperty.TryGetValue(key, out propertyInfo))
                    {
                        object result = ParseValue(propertyInfo.PropertyType, value);
                        propertyInfo.SetValue(instance, result, null);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }   
            }

            return instance;
        }
        /// <summary>
        /// Converts an object to a raw json string.
        /// </summary>
        /// <param name="item">The object to be converted.</param>
        /// <returns>Returns the raw json data.</returns>
        public static string ToJson(this object item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            AppendValue(stringBuilder, item);
            return stringBuilder.ToString();
        }

        private static void AppendValue(StringBuilder stringBuilder, object item)
        {
            if (item == null)
            {
                stringBuilder.Append("null");
                return;
            }

            Type type = item.GetType();
            if (type == typeof(string))
            {
                stringBuilder.Append('"');
                string str = (string)item;
                for (int i = 0; i < str.Length; ++i)
                    if (str[i] < ' ' || str[i] == '"' || str[i] == '\\')
                    {
                        stringBuilder.Append('\\');
                        int j = "\"\\\n\r\t\b\f".IndexOf(str[i]);
                        if (j >= 0)
                            stringBuilder.Append("\"\\nrtbf"[j]);
                        else
                            stringBuilder.AppendFormat("u{0:X4}", (UInt32)str[i]);
                    }
                    else
                        stringBuilder.Append(str[i]);
                stringBuilder.Append('"');
            }
            else if (type == typeof(byte) || type == typeof(int))
            {
                stringBuilder.Append(item.ToString());
            }
            else if (type == typeof(float))
            {
                stringBuilder.Append(((float)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(double))
            {
                stringBuilder.Append(((double)item).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (type == typeof(bool))
            {
                stringBuilder.Append(((bool)item) ? "true" : "false");
            }
            else if (item is IList)
            {
                stringBuilder.Append('[');
                bool isFirst = true;
                IList list = item as IList;
                for (int i = 0; i < list.Count; i++)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        stringBuilder.Append(',');
                    AppendValue(stringBuilder, list[i]);
                }
                stringBuilder.Append(']');
            }
            else if (item.GetType().IsArray)
            {
                stringBuilder.Append('[');
                bool isFirst = true;

                IEnumerable enumerable = item as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object element in enumerable)
                    {
                        if (isFirst)
                            isFirst = false;
                        else
                            stringBuilder.Append(',');
                        AppendValue(stringBuilder, element);
                    }
                }


                stringBuilder.Append(']');
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type keyType = type.GetGenericArguments()[0];

                //Refuse to output dictionary keys that aren't of type string
                if (keyType != typeof(string))
                {
                    stringBuilder.Append("{}");
                    return;
                }

                stringBuilder.Append('{');
                IDictionary dict = item as IDictionary;
                bool isFirst = true;
                foreach (object key in dict.Keys)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        stringBuilder.Append(',');
                    stringBuilder.Append('\"');
                    stringBuilder.Append((string)key);
                    stringBuilder.Append("\":");
                    AppendValue(stringBuilder, dict[key]);
                }
                stringBuilder.Append('}');
            }
            else if (type.IsEnum)
            {
                item = item.ToString();
                AppendValue(stringBuilder, item);
            }
            else if(type == typeof(UnityEngine.Vector2))
            {
                item = ((UnityEngine.Vector2)item).ToSerializable();
                AppendValue(stringBuilder, item);
            }
            else if (type == typeof(UnityEngine.Vector3))
            {
                item = ((UnityEngine.Vector3)item).ToSerializable();
                AppendValue(stringBuilder, item);
            }
            else if (type == typeof(UnityEngine.Quaternion))
            {
                item = ((UnityEngine.Quaternion)item).ToSerializable();
                AppendValue(stringBuilder, item);
            }
            else if (type == typeof(UnityEngine.Color))
            {
                item = ((UnityEngine.Color)item).ToSerializable();
                AppendValue(stringBuilder, item);
            }
            else if (type == typeof(Guid))
            {
                item = ((Guid)item).ToString();
                AppendValue(stringBuilder, item);
            }
            else
            {
                stringBuilder.Append('{');

                bool isFirst = true;
                FieldInfo[] fieldInfos = type.GetFields();
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    if (fieldInfos[i].IsPublic && !fieldInfos[i].IsStatic)
                    {
                        object value = fieldInfos[i].GetValue(item);
                        if (value != null)
                        {
                            if (isFirst)
                                isFirst = false;
                            else
                                stringBuilder.Append(',');
                            stringBuilder.Append('\"');
                            stringBuilder.Append(fieldInfos[i].Name);
                            stringBuilder.Append("\":");

                            AppendValue(stringBuilder, value);
                        }
                    }
                }
                PropertyInfo[] propertyInfo = type.GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (propertyInfo[i].CanRead)
                    {
                        object value = propertyInfo[i].GetValue(item, null);
                        if (value != null)
                        {
                            if (isFirst)
                                isFirst = false;
                            else
                                stringBuilder.Append(',');
                            stringBuilder.Append('\"');
                            stringBuilder.Append(propertyInfo[i].Name);
                            stringBuilder.Append("\":");

                            AppendValue(stringBuilder, value);
                        }
                    }
                }

                stringBuilder.Append('}');
            }
        }
    }
}
