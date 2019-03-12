using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebSystem.MySQLASP.NET.Models
{
    public class RequestInfo
    {
        private List<object> _models;
        private DateTime _time;
        private HttpRequestMessage _request;
        private Exception _exception;
        private double _executionTime;
        private string _sql;
        private string _content;

        public object[] Models
        {
            get { return _models.ToArray(); }
            set { _models = value.ToList(); }
        }
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public double ExecutionTime
        {
            get { return _executionTime; }
            set { _executionTime = value; }
        }
        public void Add<T>(params T[] models)
            where T: new()
        {
            _models.AddRange(models as object[]);
        }
        public string Sql
        {
            get
            {
                return _sql;
            }
            set
            {
                _sql = value;
            }
        }
        public string Url
        {
            get
            {
                return _request.RequestUri.ToString();
            }
        }
        public string Version
        {
            get
            {
                return _request.Version.ToString();
            }
        }
        public string Method
        {
            get
            {
                return _request.Method.Method;
            }
        }
        public Exception Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                _exception = value;
            }
        }

        public bool IsError
        {
            get
            {
                return _exception != null;
            }
        }
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }
        public RequestInfo(HttpRequestMessage request)
        {
            _models = new List<object>();
            _request = request;
            _time = DateTime.Now;
        }
    }
}
