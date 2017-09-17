using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class SerializableController : ApiController
    {
        public CountingLock Get(CountingLock cl)
        {
           return cl;
        }
    }

    [Serializable]
    public class CountingLock
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }
}