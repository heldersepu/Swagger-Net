using NUnit.Framework;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class ModelFilterContextTests
    {
        [Test]
        public void SchemaRegistry_Null()
        {
            var x = new ModelFilterContext(null, null, null);
            Assert.IsNull(x.SchemaRegistry);
        }
    }
}
