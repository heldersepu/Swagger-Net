using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class ProtectedResources2Controller : ApiController
    {
        [TokenAuth]
        public string Get()
        {
            throw new NotImplementedException();
        }
    }

    public class TokenAuthAttribute : Attribute
    {
    }
}