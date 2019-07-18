using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using System.Linq;

namespace Swagger.Net.Tests.Swagger
{
    [TestFixture]
    public class SchemaTestsWithCustomAuthorizeAttribute : SwaggerTestBase
    {
        public SchemaTestsWithCustomAuthorizeAttribute() : base("swagger/docs/{apiVersion}") {}

        [Test]
        public void It_adds_security_to_protected_operations()
        {
            SetUpDefaultRouteFor<ProtectedResourcesController>();
            SetUpDefaultRouteFor<ProtectedWithCustomAttributeResourcesController>();

            SetUpHandler(c =>
            {
                c.ApiKey("apiKey", "header", "API Key Authentication");
            });

            var swagger = GetContent<SwaggerDocument>(TEMP_URI.DOCS);

            var paths = swagger.paths.ToArray();

            Assert.IsNotNull(paths[0].Value.get.security);
            Assert.IsNotNull(paths[1].Value.get.security);
        }
    }
}
