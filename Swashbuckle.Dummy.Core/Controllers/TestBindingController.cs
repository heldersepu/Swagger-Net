using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Swashbuckle.Dummy.Controllers
{
    public class TestBindingController : ApiController
    {
        [Route("{id}")]
        public string Get([Bind]string id)
        {
            return "abc";
        }

        [Route("NoBind")]
        public string Get2()
        {
            return "abc";
        }
    }

    public class BindAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor p)
        {
            return new FooBinding(p);
        }
    }
    public class FooBinding : HttpParameterBinding
    {
        public FooBinding(HttpParameterDescriptor d) : base(d) { }

        public override Task ExecuteBindingAsync(ModelMetadataProvider m, HttpActionContext a, CancellationToken c)
        {
            return Task.FromResult(true);
        }
    }
}
