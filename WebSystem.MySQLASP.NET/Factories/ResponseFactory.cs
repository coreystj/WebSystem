using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace WebSystem.MySQLASP.NET.Factories
{
    public static class ResponseFactory
    {
        public static HttpResponseMessage Create(this HttpRequestMessage request, 
            Dictionary<string, object> arguments, object data)
        {
            arguments["Data"] = data;

            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(
                JsonConvert.SerializeObject(arguments, Formatting.Indented), Encoding.UTF8, "application/json");

            return response;
        }

        public static HttpResponseMessage CreateError(this HttpRequestMessage request, 
            Dictionary<string, object> arguments, Exception exception)
        {
            arguments["Error"] = new
            {
                Message = exception.Message,
                Stacktrace = exception.StackTrace
            };

            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(
                JsonConvert.SerializeObject(arguments, Formatting.Indented), Encoding.UTF8, "application/json");

            return response;
        }
    }
}