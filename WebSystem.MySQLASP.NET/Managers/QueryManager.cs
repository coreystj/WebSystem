using MySql.Data.MySqlClient;
using WebSystem.MySQLASP.NET.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Xml.Serialization;

namespace WebSystem.MySQLASP.NET.Managers
{
    public static class QueryManager
    {
        //server=localhost;user=root;database=dbname;port=3306;password=usbw;SslMode=none
        private static string _connectionString;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                    throw new Exception("Database connection string is null or empty. " +
                        "Please set the connection string before continuing.");
                else
                    return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public static IEnumerable<MySqlDataReader> QueryReader(
            string sql, Action onSuccess = null, Action<Exception> onFailed = null)
        {
            MySqlCommand command = null;
            MySqlDataReader dataReader = null;

            try
            {
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open();
                command = new MySqlCommand(sql, connection);
                dataReader = command.ExecuteReader();
            }
            catch(Exception ex)
            {
                if (onFailed != null)
                    onFailed(ex);
                else
                    throw ex;
            }

            if (dataReader != null && dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    yield return dataReader;
                }

                dataReader.Close();
                onSuccess?.Invoke();
            }
        }

        public static string GetContent(this HttpRequestMessage request)
        {
            string jsonContent = request.Content.ReadAsStringAsync().Result;
            return jsonContent;
        }

        public static Dictionary<string, object> GetJson(this HttpRequestMessage request)
        {
            string jsonContent = request.GetContent();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
        }

        public static string Create<T>(params T[] newItems)
            where T : new()
        {
            string sql = string.Empty;
            foreach (T newItem in newItems)
            {
                sql = GetInsertQuery<T>(newItem);
                newItem.SetValue("Id", InsertQuery(sql));
            }

            return sql;
        }

        public static string GetInsertQuery<T>(T item)
                        where T : new()
        {
            string className = GetPreferredName(item.GetType());
            string[] propertyNames = GetPropertyNames(item);
            object[] propertyValues = GetPropertyValues(item);

            string XmlElements = string.Empty;
            string values = string.Empty;
            foreach (string propertyName in propertyNames)
            {
                XmlElements += ", `" + propertyName + "`";
            }

            foreach (object propertyValue in propertyValues)
            {
                values += ", '" + propertyValue + "'";
            }

            string sql = $"INSERT INTO {className} (`id`{XmlElements}) VALUES (NULL{values});";

            return sql;
        }

        public static string Update<T>(params T[] newItems)
            where T : new()
        {
            string sql = string.Empty;
            foreach (T newItem in newItems)
            {
                sql = GetUpdateQuery<T>(newItem);
                UpdateDeleteQuery(sql);
            }

            return sql;
        }

        public static string GetUpdateQuery<T>(T item)
                        where T : new()
        {
            string className = GetPreferredName(item.GetType());
            string[] propertyNames = GetPropertyNames(item);
            object[] propertyValues = GetPropertyValues(item);

            string result = string.Empty;
            string propertyName;
            object propertyValue;

            for (int i = 0; i < propertyNames.Length; i++)
            {
                propertyName = propertyNames[i];
                propertyValue = propertyValues[i];

                result += $@"`{propertyName}` = '{propertyValue}'" + ((i == propertyNames.Length - 1) ? " " : ", ");
            }

            string sql = $"UPDATE {className} SET {result} WHERE `id` = {item.GetValue<int>("Id")};";

            return sql;
        }

        public static string Delete<T>(params T[] newItems)
            where T : new()
        {
            string sql = string.Empty;
            foreach (T newItem in newItems)
            {
                sql = GetDeleteQuery<T>(newItem);
                UpdateDeleteQuery(sql);
            }

            return sql;
        }

        public static string GetDeleteQuery<T>(T item)
                        where T : new()
        {
            string className = GetPreferredName(item.GetType());

            string sql = $"DELETE FROM {className} WHERE `id` = {item.GetValue<int>("Id")};";

            return sql;
        }

        private static void SetValue(this object model, string columnName, object value)
        {
            PropertyInfo property = model.GetType().GetProperty(columnName);
            property.SetValue(model, value);
        }

        private static T GetValue<T>(this object model, string columnName)
        {
            PropertyInfo property = model.GetType().GetProperty(columnName);
            object value = property.GetValue(model);
            return (T)value;
        }

        public static string Read<T>(int id, out T[] results)
            where T : new()
        {   
            string sql = string.Format("SELECT * FROM {0} WHERE id = '{1}'", typeof(T).Name, id);
            results = Query<T>(sql);
            return sql;
        }

        public static string Read<T>(out T[] results)
            where T : new()
        {
            string sql = string.Format("SELECT * FROM {0}", typeof(T).Name);
            results = Query<T>(sql);
            return sql;
        }

        public static string Read<T>(string sql, out T[] results)
            where T : new()
        {
            results = Query<T>(sql);
            return sql;
        }


        public static T[] Query<T>(string sql)
            where T : new()
        {
            var records = new List<T>();
            var reader = QueryManager.QueryReader(sql);

            foreach (MySqlDataReader record in reader)
            {
                records.Add(Get<T>(record));
            }

            return records.ToArray();
        }

        public static T Get<T>(MySqlDataReader record)
            where T : new()
        {
            var tmp = new T();
            for (int i = 0; i < record.FieldCount; i++)
            {
                PropertyInfo[] properties = tmp.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (record.GetName(i) == GetPreferredName(property))
                    {
                        if(property.PropertyType == typeof(DateTime))
                            property.SetValue(tmp, record.GetDateTime(i));
                        else
                            property.SetValue(tmp, record.GetValue(i));
                        break;
                    }
                }
            }
            return tmp;
        }

        public static string GetPreferredName(PropertyInfo property)
        {
            XmlElementAttribute preferredAttribute = property.GetCustomAttributes<XmlElementAttribute>(false)
                .FirstOrDefault();
            return (preferredAttribute == null) ? property.Name : preferredAttribute.ElementName;
        }

        public static string[] GetPropertyNames<T>(T item, bool omitId = true)
            where T : new()
        {
            
            PropertyInfo[] properties = item.GetType().GetProperties();
            if (omitId)
                properties = properties.Where(x => x.Name.ToUpper() != "ID").ToArray();
            string[] names = new string[properties.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = GetPreferredName(properties[i]);
            }

            return names;
        }

        public static object[] GetPropertyValues<T>(T item, bool omitId = true)
            where T: new()
        {

            PropertyInfo[] properties = item.GetType().GetProperties();
            if (omitId)
                properties = properties.Where(x => x.Name.ToUpper() != "ID").ToArray();
            object[] names = new object[properties.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = properties[i].GetValue(item).ToString();
            }

            return names;
        }

        public static string GetPreferredName(Type obj)
        {
            XmlElementAttribute preferredAttribute = obj.GetCustomAttributes<XmlElementAttribute>(false)
                .FirstOrDefault();
            return (preferredAttribute == null) ? obj.Name : preferredAttribute.ElementName;
        }

        public static T QueryOne<T>(string sql)
            where T : new()
        {
            return Query<T>(sql).FirstOrDefault();
        }

        public static void UpdateDeleteQuery(string sql)
        {
            MySqlCommand command = null;

            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        public static int InsertQuery(string sql)
        {
            MySqlCommand command = null;

            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();

            return (int)command.LastInsertedId;
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
                        if (key == "id" || key == "Id")
                        {
                            if(dict[key] is long)
                                property.SetValue(tmp, (int)((long)dict[key]));
                            else if (dict[key] is int)
                                property.SetValue(tmp, (int)dict[key]);
                            else if (dict[key] is string)
                                property.SetValue(tmp, int.Parse(dict[key] as string));
                        }
                        else
                            property.SetValue(tmp, dict[key]);
                        break;
                    }
                }
            }

            return tmp;
        }

        public static RequestInfo DoTry(this HttpRequestMessage request, Action<RequestInfo> action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var info = new RequestInfo(request);
            try
            {
                action(info);
            }
            catch (Exception ex)
            {
                info.Exception = ex;
            }
            stopwatch.Stop();
            info.ExecutionTime = stopwatch.Elapsed.TotalSeconds;
            return info;
        }
    }
}