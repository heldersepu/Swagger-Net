using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public abstract class Blob<T> : ApiController
    {
        /// <summary>GET</summary>
        public int Get(int? x) { return 0; }

        /// <summary>POST</summary>
        public int Post(int x) { return 0; }
    }

    public class Foo { }

    public class BlobController : Blob<Foo> { }
}
