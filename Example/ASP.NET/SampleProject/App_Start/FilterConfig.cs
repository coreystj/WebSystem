using System.Web;
using System.Web.Mvc;

namespace WebSystem.Example.ASP.NET.SampleProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
