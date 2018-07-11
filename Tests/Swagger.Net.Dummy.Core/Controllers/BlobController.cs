using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public abstract class Blob<T1, T2> : ApiController
    {
        /// <summary>GET</summary>
        public int Get(int? x) { return 0; }

        /// <summary>POST</summary>
        public int Post(int x) { return 0; }

        /// <summary>PUT</summary>
        public int Put(AnotherFoo<T1> x) { return 0; }

        /// <summary>PATCH</summary>
        public int Patch(T2 y, T1 x) { return 0; }
    }

    public class Foo { }
    public class Bar { }

    public class AnotherFoo<TV> { }

    public class BlobController : Blob<Foo, Bar> { }
}
