using System.Web.Http;
using System.Collections.Generic;
using Swagger.Net.Dummy.Types;

namespace Swagger.Net.Dummy.Controllers
{
    public class ArraysTestController : ApiController
    {
        [Route("list_string")]
        public List<string> Get_list_string([FromUri] List<string> p)
        {
            return p;
        }

        [Route("list_integer")]
        public List<int> Get_list_int([FromUri] List<int> p)
        {
            return p;
        }

        [Route("list_location")]
        public List<Location> Get_list_location([FromUri] List<Location> p)
        {
            return p;
        }
    }
}