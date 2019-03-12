using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebSystem.MySQLASP.NET.Managers;

namespace WebSystem.Example.ASP.NET.SampleProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            QueryManager.ConnectionString = "server=localhost;user=root;database=sampledatabase;port=3306;password=usbw;SslMode=none";

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
