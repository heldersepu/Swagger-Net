using NUnit.Framework;
using Swagger.Net.FromUriParams;
using System;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class HandleFromUriParamsTests
    {
        [Test]
        public void ExtractAndAddQueryParams_null()
        {
            var target = new HandleFromUriParams();
            Assert.Throws<NullReferenceException>(() => target.ExtractAndAddQueryParams(null, "", false, null, null));
        }

        [Test]
        public void ExtractAndAddQueryParams_empty()
        {
            var target = new HandleFromUriParams();
            var schema = new Schema();
            Assert.DoesNotThrow(() => target.ExtractAndAddQueryParams(schema, "", false, null, null));
        }
    }
}
