using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Web
{

    public class WebContext
    {
        private string _url;
        private Action<string> _success;
        private Action<Exception> _failed;
        private Dictionary<string, object> _form;
        private Dictionary<string, string> _headers;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public Action<string> Success
        {
            get { return _success; }
            set { _success = value; }
        }

        public Action<Exception> Failed
        {
            get { return _failed; }
            set { _failed = value; }
        }

        public Dictionary<string, string> Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }
        public Dictionary<string, object> Form
        {
            get { return _form; }
            set { _form = value; }
        }

        public WebContext(string url, Dictionary<string, string> headers,
            Action<string> success, Action<Exception> failed, Dictionary<string, object> form = null)
        {
            _url = url;
            _headers = headers;
            _success = success;
            _failed = failed;
            _form = form;
        }
    }
}
