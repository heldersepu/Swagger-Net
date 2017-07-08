using NUnit.Framework;
using Swagger.Net.Swagger.Annotations;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class ApplySwaggerResponseAttributesTests
    {
        [Test]
        public void InferDescriptionFrom_Empy()
        {
            string x = new ApplySwaggerResponseAttributes().InferDescriptionFrom(string.Empty);
            Assert.IsNull(x);
        }

        [Test]
        public void InferDescriptionFrom_Good()
        {
            string x = new ApplySwaggerResponseAttributes().InferDescriptionFrom("404");
            Assert.AreEqual("NotFound", x);
        }

        [Test]
        public void InferDescriptionFrom_Bad()
        {
            string x = new ApplySwaggerResponseAttributes().InferDescriptionFrom("asdf");
            Assert.IsNull(x);
        }
    }
}
