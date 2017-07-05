using NUnit.Framework;
using Swashbuckle.Application;
using System.Net.Http;

namespace Swashbuckle.Tests.CoreUnitTests
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
