using NUnit.Framework;
using Swagger.Net.Swagger;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class StringExtensionsTests
    {
        [Test]
        public void ToCamelCase_null()
        {
            Assert.AreEqual(string.Empty, string.Empty.ToCamelCase());
        }

        [Test]
        public void ToTitleCase_null()
        {
            Assert.AreEqual(string.Empty, string.Empty.ToTitleCase());
        }
    }
}
