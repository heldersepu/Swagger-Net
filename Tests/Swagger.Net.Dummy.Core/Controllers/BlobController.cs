using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public abstract class Blob<T> : ApiController
    {
        /// <summary>GET</summary>
        public int Get(int? x) { return 0; }

        /// <summary>POST</summary>
        public int Post(int x) { return 0; }

        /// <summary>PUT</summary>
        public int Put(AnotherFoo<T> x) { return 0; }
    }

    public abstract class Blob<T, TSecond, TThird> : Blob<T>
    {
        /// <summary>PATCH</summary>
        public int Patch(AnotherFoo<TThird> x, T y) { return 0; }
    }

    public class Foo { }

    public class FooBar { }

    public class Bar { }

    public class AnotherFoo<TV> { }

    public class BlobController : Blob<Foo> { }

    public class BlobControllerThree : Blob<Foo, FooBar, Bar> { }
}
