using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    [TokenClassAuth]
    public class ProtectedResources3Controller : ApiController
    {
        public string Get()
        {
            throw new NotImplementedException();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TokenClassAuthAttribute : Attribute
    {
    }
}