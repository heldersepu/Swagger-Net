using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;


namespace Swagger.Net.Tests.Swagger
{
    [TestFixture]
    public class SerializableTests : SwaggerTestBase
    {
        public SerializableTests() : base("swagger/docs/{apiVersion}") { }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultRouteFor<SerializableController>();

            // Default set-up
            SetUpHandler();
        }

        [Test]
        public void It_assigns_operation_properties_from_swagger_operation_attribute()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var paths = swagger["paths"];
            Assert.IsNotNull(paths);
        }
    }
}
