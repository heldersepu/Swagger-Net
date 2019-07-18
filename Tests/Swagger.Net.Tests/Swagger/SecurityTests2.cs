using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using Swagger.Net.Tests.Swagger;

namespace Swagger.Net.Tests.SwaggerFilters
{
    [TestFixture]
    public class SecurityTests2 : SwaggerTestBase
    {
        public SecurityTests2() : base("swagger/docs/{apiVersion}") { }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultRouteFor<ProtectedResources2Controller>();
        }

        [Test]
        public void It_exposes_securityDefinitions()
        {
            SetUpHandler(c =>
            {
                c.ApiKey("token", "header", "TokenAuth", typeof(TokenAuthAttribute));
            });

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var securityDefinitions = swagger["securityDefinitions"];
            var expected = JObject.FromObject(new
            {
                token = new
                {
                    type = "apiKey",
                    description = "TokenAuth",
                    name = "token",
                    @in = "header",
                },
            });

            Assert.AreEqual(expected.ToString(), securityDefinitions.ToString());
        }
    }
}