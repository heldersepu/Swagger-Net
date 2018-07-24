using System.Collections.Generic;
using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public abstract class Blob<T1, T2, T3> : ApiController
    {
        /// <summary>GET</summary>
        public int Get(int? x) { return 0; }

        /// <summary>POST</summary>
        public int Post(int x) { return 0; }

        /// <summary>PUT</summary>
        public int Put(AnotherFoo<T1> x) { return 0; }

        /// <summary>PATCH</summary>
        public int Patch(AnotherFoo<T2> y, IEnumerable<T3> x) { return 0; }
    }

    public class Foo { }
    public class Bar { }
    public class Derp { }

    public class AnotherFoo<TV> { }

    public class BlobController : Blob<Foo, Bar, Derp> { }
}
