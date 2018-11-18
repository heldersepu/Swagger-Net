using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class ProtectedResourcesController : ApiController
    {
        [Authorize(Roles = "read")]
        public JObject Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class ProtectedWithCustomAttributeResourcesController : ApiController
    {
        [AuthorizedRoles(Role.Read,Role.Write)]
        public JObject Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class AuthorizedRolesAttribute : AuthorizeAttribute
    {
        public AuthorizedRolesAttribute(params Role[] roles) : base()
        {
            Roles = String.Join(",", Enum.GetNames(typeof(Role)));
        }
    }

    public enum Role
    {
        Read, Write, Delete
    }

}