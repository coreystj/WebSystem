
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using WebSystem.MySQLUnity.IO;

namespace WebSystem.MySQLUnity.Web
{
    public class InternalWeb : WebInfo
    {
        public override IEnumerator GetText(string url, Dictionary<string, string> headers, 
            Action<string> success, Action<Exception> failed)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);

            foreach (string header in headers.Keys)
            {
                www.SetRequestHeader(header, headers[header]);
            }

            www.SendWebRequest();
            while (!www.isDone)
            {
                yield return null;

            }
            HandleRequest(www, success, failed);
        }

        public override IEnumerator PostText(string url, Dictionary<string, string> headers, 
            Action<string> success, Action<Exception> failed, Dictionary<string, object> form)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, JsonConvert.ToJson(form));
            UploadHandlerRaw upHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.ToJson(form)));
            upHandler.contentType = "application/x-www-form-urlencoded";
            www.uploadHandler = upHandler;

            foreach (string header in headers.Keys)
            {
                www.SetRequestHeader(header, headers[header]);
            }
            www.SendWebRequest();
            while (!www.isDone)
            {
                yield return null;

            }
            HandleRequest(www, success, failed);
        }

        private void HandleRequest(UnityWebRequest www, Action<string> success, Action<Exception> failed)
        {
            _isRequesting = false;
            string[] keys = _queue.Keys.ToArray();
            _queue.Remove(keys[0]);
            if (www.isNetworkError || www.isHttpError)
            {
                failed(new Exception(www.url + ": " 
                    + www.downloadHandler.text + ": " + www.error));
            }
            else
            {
                try
                {
                    success(www.downloadHandler.text);
                }
                catch(Exception ex)
                {
                    failed(ex);
                }
            }
        }
    }
}
