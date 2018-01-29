using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public class AttributeRoutesController : ApiController
    {
        [Route("subscriptions/{id}/cancel")]
        public void CancelSubscription(int id)
        {
            throw new NotImplementedException();
        }

        [Route("post/one")]
        [Route("post/two")]
        [Route("post/abc")]
        public void Post(int id)
        {
            throw new NotImplementedException();
        }
    }
}