using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class ConflictingActionsController : ApiController
    {
        public IEnumerable<string> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllByKeyword(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}