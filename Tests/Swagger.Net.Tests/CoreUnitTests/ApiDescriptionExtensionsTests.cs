using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace Swagger.Net.Tests.CoreUnitTests
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

        [Test]
        public void GetUniqueFriendlyId_Test()
        {
            var apiDesc = apiDescription("asdf", "POST");
            string friendlyId = apiDesc.FriendlyId();
            string friendlyId2 = apiDesc.FriendlyId2();
            Assert.AreNotEqual(friendlyId, friendlyId2);

            var operationNames = new HashSet<string> { friendlyId, friendlyId2 };
            var swaggerGenerator = new SwaggerGenerator(null, null, null);
            string uniqueFriendlyId = swaggerGenerator.GetUniqueOperationId(apiDesc, operationNames);
            Assert.AreNotEqual(friendlyId2, uniqueFriendlyId);
        }
    }
}
