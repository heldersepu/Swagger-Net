using System;
using System.Web.Http;

namespace Swashbuckle.Dummy.Controllers
{
    public class AttributeRoutesController : ApiController
    {
        [Route("subscriptions/{id}/cancel")]
        public void CancelSubscription(int id)
        {
            throw new NotImplementedException();
        }
    }
}