using NUnit.Framework;
using Swagger.Net.Application;
using System.Net.Http;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class RedirectHandlerTests
    {
        internal string GetRootUrl(HttpRequestMessage r)
        {
            return "http://tempuri.org";
        }

        [Test]
        public void GetAsset_null()
        {
            var redirect = new RedirectHandler(GetRootUrl, "swagger");
            Assert.DoesNotThrow(() => redirect.SendAsync(new HttpRequestMessage()));
        }
    }
}
