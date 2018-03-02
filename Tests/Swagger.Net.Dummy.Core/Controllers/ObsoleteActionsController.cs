using System;
using System.Web.Http;

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
    }
}