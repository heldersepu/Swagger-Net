using System;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public abstract class BaseController<T> : ApiController
    {

        /// <summary>
        /// Get a record by the given key.
        /// </summary>
        public virtual T Get(T id)
        {
            return id;
        }

        /// <summary>
        /// Post a record.
        /// </summary>
        public virtual long Post([FromBody]string value)
        {
            return DateTime.Now.Ticks;
        }

        /// <summary>
        /// Put a record by the given key.
        /// </summary>
        public virtual T Put(T id, [FromBody]string value)
        {
            return id;
        }

        /// <summary>
        /// Delete a record by the given key.
        /// </summary>
        public virtual long Delete(long id)
        {
            return id;
        }
    }
}
