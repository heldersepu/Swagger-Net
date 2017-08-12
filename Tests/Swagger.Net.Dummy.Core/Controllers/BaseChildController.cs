using System.Collections.Generic;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class BaseChildController : BaseController<long>
    {
        /// <summary>
        /// Get some ...
        /// </summary>
        public IEnumerable<string> Get()
        {
            return new string[] { "aei", "bcd" };
        }
    }
}
