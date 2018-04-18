using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class FromBodyParamsController : ApiController
    {
        [HttpGet]
        public Transaction Get([FromBody] Transaction t)
        {
            return t;
        }
    }
}