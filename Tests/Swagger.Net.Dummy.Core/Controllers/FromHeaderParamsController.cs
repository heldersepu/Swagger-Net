using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class FromHeaderParamsController : ApiController
    {
        [HttpGet]
        public string Get([FromBody] string t)
        {
            return t;
        }
    }
}