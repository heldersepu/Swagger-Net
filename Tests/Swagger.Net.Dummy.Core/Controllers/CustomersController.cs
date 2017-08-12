using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    [RoutePrefix("Customers")]
    public class CustomersController : ApiController
    {
        public int Create(Customer customer, int adminLevel = 0)
        {
            throw new NotImplementedException();
        }

        [Route("{id?}")]
        public string Get(int id = 8)
        {
            return $"{id}";
        }

        [HttpPut]
        [Route("{id}")]
        public void Update(int id, Customer customer)
        {
            throw new NotImplementedException();
        }

        [Route("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}