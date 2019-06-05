using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    
    public class NestingBaseClassController<T> : ApiController
    {
        public class NestedClass
        {
            /// <summary>
            /// Property that is of generic type taken from the class that this class is nested into
            /// </summary>
            public T GenericallyTypedProperty { get; set; }
        }
    }

    public class NestingClassController : NestingBaseClassController<string>
    {
        public NestedClass Get(int x)
        {
            return new NestedClass();
        }
    }
}
