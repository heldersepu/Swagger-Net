using NUnit.Framework;
using Swagger.Net.Application;
using System;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class VendorExtensionsConverterTests
    {
        [Test]
        public void ReadJson_Test()
        {
            var obj = new VendorExtensionsConverter();
            Assert.Throws<NotImplementedException>(() => obj.ReadJson(null, null, null, null));
        }
    }
}
