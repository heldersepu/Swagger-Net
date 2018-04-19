using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using System.Linq;

namespace Swagger.Net.Tests.Swagger
{
    [TestFixture]
    public class FromHeaderParamsTests : SwaggerTestBase
    {
        public FromHeaderParamsTests() : base("swagger/docs/{apiVersion}") { }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultRouteFor<FromHeaderParamsController>();
            SetUpHandler();
        }

        [Test]
        public void It_sets_headers()
        {
            var swagger = GetContent<SwaggerDocument>(TEMP_URI.DOCS);
            foreach (var path in swagger.paths)
            {
                validate(path.Value.get);
                validate(path.Value.post);
            }

            void validate(Operation op)
            {
                foreach(var param in op.parameters)
                    Assert.AreEqual("header", param.@in);
            }
        }

        [Test]
        public void It_sets_headers_with_different_name()
        {
            var swagger = GetContent<SwaggerDocument>(TEMP_URI.DOCS);
            Assert.AreEqual("abctest", swagger.paths.First().Value.post.parameters.First().name);
        }
    }
}