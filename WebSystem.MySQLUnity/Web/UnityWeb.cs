
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WebSystem.MySQLUnity.Web
{
    public static class UnityWeb
    {
        public static Dictionary<string, string> Headers
        {
            get
            {
                var headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Credentials", "true" },
                    { "Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time"},
                    { "Access-Control-Allow-Methods", "GET, POST, OPTIONS"},
                    {"Access-Control-Allow-Origin", "*"}
                };
                return headers;
            }
        }

        private static WebInfo _instance;
        private static WebInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject();
                    obj.name = "UnityWeb";
                    _instance = obj.AddComponent<InternalWeb>();
                    _instance.Initialize();
                }
                return _instance;
            }
        }

        public static void Post(string url,
            Dictionary<string, object> form, Action<string> success, Action<Exception> failed)
        {
            Instance.Post(url, Headers, form, success, failed);
        }

        public static void Get(string url,
            Action<string> success, Action<Exception> failed)
        {
            Instance.Get(url, Headers, success, failed);
        }
    }
}
