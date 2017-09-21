using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;

namespace Swagger.Net.Tests.Swagger
{
    [TestFixture]
    public class XmlComments2Tests : SwaggerTestBase
    {
        public XmlComments2Tests() : base("swagger/docs/{apiVersion}") { }

        [SetUp]
        public void SetUp()
        {
            SetUpAttributeRoutesFrom(typeof(BlobController).Assembly);
            SetUpDefaultRouteFor<BlobController>();
            SetUpHandler();
        }

        [Test]
        public void It_documents_operations_from_inherited_actions()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            Assert.IsNotNull(swagger);

            var path = swagger["paths"]["/blob"];
            Assert.IsNotNull(path);

            Assert.IsNotNull(path["post"]["summary"]);
            Assert.IsNotNull(path["get"]["summary"]);
        }
    }
}