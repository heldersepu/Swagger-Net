using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class FromHeaderParamsController : ApiController
    {
        [HttpGet]
        public string Get([FromHeader] string test)
        {
            return test;
        }
    }
}