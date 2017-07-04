using NUnit.Framework;
using Swashbuckle.Swagger;

namespace Swashbuckle.Tests.CoreUnitTests
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
