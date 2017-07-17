using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class PathRequiredController : ApiController
    {
        public void Get(long id = 0)
        {
            throw new NotImplementedException();
        }

        public void Post(ComplexObject1 co)
        {
            throw new NotImplementedException();
        }

        public void Put(ComplexObject2 co)
        {
            throw new NotImplementedException();
        }
    }

    public class ComplexObject1
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class ComplexObject2
    {
        [Required]
        public int Id;
        [Required]
        public string Name;
    }
}
