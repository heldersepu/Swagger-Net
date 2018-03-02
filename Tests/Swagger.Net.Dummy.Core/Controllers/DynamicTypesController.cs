using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class DynamicTypesController : ApiController
    {
        public int Create(dynamic thing)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public void Update(DynamicObjectSubType resource)
        {
        }

        public ExpandoObject GeByProperties(JObject thing)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JToken> GetAll()
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IHttpActionResult Head()
        {
            throw new NotImplementedException();
        }
    }

    [JsonObject]
    public class DynamicObjectSubType : DynamicObject
    {
        [Range(1.1, 9.9)]
        public double Id;

        public string Name { get; set; }
    }
}