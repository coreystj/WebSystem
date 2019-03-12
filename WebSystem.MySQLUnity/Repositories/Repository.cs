using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSystem.MySQLUnity.Helpers;
using WebSystem.MySQLUnity.IO;
using WebSystem.MySQLUnity.Web;

namespace WebSystem.MySQLUnity.Repositories
{
    public static class Repository
    {
        public static void Read<T>(string url, int id, Action<T> onSuccess, Action<Exception> onFailed)
            where T : new()
        {
            UnityWeb.Get(string.Format(url + "api/{0}/read/{1}", typeof(T).Name, id), (string rawText)=> {
                var json = JsonConvert.ToObject<Dictionary<string, object>>(rawText);
                json.GetException();
                T[] models = json.GetValues<T>("Models");
                onSuccess(models.FirstOrDefault());
            }, onFailed);
        }

        public static void Read<T>(string url, Action<T[]> onSuccess, Action<Exception> onFailed)
            where T : new()
        {
            UnityWeb.Get(string.Format(url + "api/{0}/read", typeof(T).Name), (string rawText) => {
                var json = JsonConvert.ToObject<Dictionary<string, object>>(rawText);
                json.GetException();
                T[] models = json.GetValues<T>("Models");
                onSuccess(models);
            }, onFailed);
        }

        public static void Create<T>(string url, T model, Action<T> onSuccess, Action<Exception> onFailed)
            where T : new()
        {
            UnityWeb.Post(string.Format(url + "api/{0}/create", typeof(T).Name), model.Serialize(), (string rawText) => {
                var json = JsonConvert.ToObject<Dictionary<string, object>>(rawText);
                json.GetException();
                T[] models = json.GetValues<T>("Models");
                onSuccess(models.FirstOrDefault());
            }, onFailed);
        }

        public static void Update<T>(string url, T model, Action<T> onSuccess, Action<Exception> onFailed)
            where T : new()
        {
            UnityWeb.Post(string.Format(url + "api/{0}/update", typeof(T).Name), model.Serialize(), (string rawText) => {
                var json = JsonConvert.ToObject<Dictionary<string, object>>(rawText);
                json.GetException();
                T[] models = json.GetValues<T>("Models");
                onSuccess(models.FirstOrDefault());
            }, onFailed);
        }

        public static void Delete<T>(string url, T model, Action onSuccess, Action<Exception> onFailed)
            where T : new()
        {
            UnityWeb.Post(string.Format(url + "api/{0}/delete", typeof(T).Name), model.Serialize(), (string rawText) => {
                var json = JsonConvert.ToObject<Dictionary<string, object>>(rawText);
                json.GetException();
                onSuccess();
            }, onFailed);
        }
    }
}
