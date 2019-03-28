using Swagger.Net.Swagger.Annotations;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Swagger.Net.Dummy.Controllers
{
    public class ObsoleteActionsController : ApiController
    {
        [HttpPut]
        public int Update(int id, string value)
        {
            throw new NotImplementedException();
        }

        [Obsolete("OBSOLETE_DELETE")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        [SwaggerDescription(summary: "abc")]
        [Obsolete("OBSOLETE_POST")]
        public void Post(int id)
        {
            throw new NotImplementedException();
        }

        [SwaggerDescription("abc", "def")]
        [Obsolete("OBSOLETE_POST")]
        public void Patch(int id)
        {
            throw new NotImplementedException();
        }

        [Obsolete()]
        [ResponseType(typeof(int))]
        public int Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}