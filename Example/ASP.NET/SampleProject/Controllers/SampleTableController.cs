using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSystem.Example.SampleProject.Models;
using WebSystem.MySQLASP.NET.Helpers;
using WebSystem.MySQLASP.NET.Managers;
using WebSystem.MySQLASP.NET.Models;

namespace WebSystem.Example.ASP.NET.SampleProject.Controllers
{
    public class SampleTableController : ApiController
    {

        [HttpGet]
        [Route("api/sampletable/read/{id}")]
        public RequestInfo Read(int id)
        {
            return Request.DoTry((RequestInfo info) => {
                info.Sql = QueryManager.Read(id, out SampleTable[] models);
                info.Add(models);
            });
        }

        [HttpGet]
        [Route("api/sampletable/read")]
        public RequestInfo Read()
        {
            return Request.DoTry((RequestInfo info) => {
                info.Sql = QueryManager.Read(out SampleTable[] models);
                info.Add(models);
            });
        }

        [HttpPost]
        [Route("api/sampletable/create")]
        public RequestInfo Create()
        {
            return Request.DoTry((RequestInfo info) => {
                SampleTable model = Request.Get<SampleTable>();
                info.Sql = QueryManager.Create(model);
                info.Add(model);
            });
        }

        [HttpPost]
        [Route("api/sampletable/update")]
        public RequestInfo Update()
        {
            return Request.DoTry((RequestInfo info) => {
                SampleTable model = Request.Get<SampleTable>();
                info.Sql = QueryManager.Update(model);
                info.Add(model);
            });
        }

        [HttpPost]
        [Route("api/sampletable/delete")]
        public RequestInfo Delete()
        {
            return Request.DoTry((RequestInfo info) => {
                SampleTable model = Request.Get<SampleTable>();
                info.Sql = QueryManager.Delete(model);
                info.Add(model);
            });
        }
    }
}
