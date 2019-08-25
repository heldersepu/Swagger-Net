using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using Swagger.Net.Tests.Swagger;

namespace Swagger.Net.Tests.SwaggerFilters
{
    [TestFixture]
    public class SecurityTests3 : SwaggerTestBase
    {
        public SecurityTests3() : base("swagger/docs/{apiVersion}") { }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultRouteFor<ProtectedResources3Controller>();
        }

        [Test]
        public void It_exposes_securityDefinitions()
        {
            SetUpHandler(c =>
            {
                c.ApiKey("token", "header", "TokenAuth", typeof(TokenClassAuthAttribute));
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

        [Test]
        public void It_exposes_security_on_action()
        {
            SetUpHandler(c =>
            {
                c.ApiKey("token", "header", "TokenAuth", typeof(TokenClassAuthAttribute));
            });

            var swagger = GetContent<SwaggerDocument>(TEMP_URI.DOCS);

            Assert.IsNotNull(swagger.paths["/protectedresources3"].get.security);
        }
    }
}
