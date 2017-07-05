using Moq;
using NUnit.Framework;
using Swashbuckle.Swagger;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace Swashbuckle.Tests.CoreUnitTests
{
    [TestFixture]
    class ApiDescriptionExtensionsTests
    {
        private ApiDescription apiDescription(string path, string method)
        {
            var mock = new Mock<HttpActionDescriptor>();
            mock.Object.ControllerDescriptor = new HttpControllerDescriptor();

            return new ApiDescription()
            {
                RelativePath = path,
                HttpMethod = new HttpMethod(method),
                ActionDescriptor = mock.Object
            };
        }

        [TestCase("_PostAsdf", "asdf", "POST")]
        [TestCase("_PostByAsdf", "{asdf", "POST")]
        [TestCase("_PostByIdByCo", "{id}/{co}", "POST")]
        public void FriendlyId2_Test(string expected, string path, string method)
        {
            Assert.AreEqual(expected, apiDescription(path, method).FriendlyId2());
        }

        [Test]
        public void ResponseType_Test()
        {
            Assert.IsNull(apiDescription(null, "POST").ResponseType());
        }
    }
}
