using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class ObsoleteEnumsController : ApiController
    {
        public int Get(Colors color)
        {
            throw new NotImplementedException();
        }
    }

    public enum Colors
    {
        [Obsolete]
        Red = 0,

        [Obsolete]
        Blue = 1,

        Green = 2
    }
}
