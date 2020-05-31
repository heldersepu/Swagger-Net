using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class RangeAttribController : ApiController
    {
        public int Get([Range(1, 9)] int id)
        {
            throw new NotImplementedException();
        }
    }
}