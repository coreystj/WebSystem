using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace WebSystem.MySQLUnity.Web
{
    public abstract class WebInfo : MonoBehaviour
    {
        public static WebInfo Instance;

        protected Dictionary<string, WebContext> _queue;
        protected bool _isRequesting;

        public void Initialize()
        {
            Instance = this;
            _queue = new Dictionary<string, WebContext>();
            _isRequesting = false;
        }

        private void Update()
        {
            string[] keys = _queue.Keys.ToArray();
            if (keys.Length > 0 && !_isRequesting)
            {
                _isRequesting = true;
                Request(_queue[keys[0]]);
            }
        }
        private void Request(WebContext webContext)
        {
            if (webContext.Form == null)
                StartCoroutine(GetText(webContext.Url, webContext.Headers,
                    webContext.Success, webContext.Failed));
            else
                StartCoroutine(PostText(webContext.Url, webContext.Headers,
                    webContext.Success, webContext.Failed, webContext.Form));
        }

        private void GetRequest(string url, Dictionary<string, string> headers,
            Action<string> success, Action<Exception> failed)
        {
            StartCoroutine(GetText(url, headers, success, failed));
        }

        public void Get(string url, Dictionary<string, string> headers,
            Action<string> success, Action<Exception> failed)
        {
            _queue[url] = new WebContext(url, headers, success, failed);
        }

        public void Post(string url, Dictionary<string, string> headers,
            Dictionary<string, object> form, Action<string> success, Action<Exception> failed)
        {
            _queue[url] = new WebContext(url, headers, success, failed, form);
        }

        public abstract IEnumerator GetText(string url, Dictionary<string, string> headers,
            Action<string> success, Action<Exception> failed);

        public abstract IEnumerator PostText(string url, Dictionary<string, string> headers,
            Action<string> success, Action<Exception> failed, Dictionary<string, object> form);
    }
}
