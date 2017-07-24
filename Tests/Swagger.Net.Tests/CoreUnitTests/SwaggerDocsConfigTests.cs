using NUnit.Framework;
using Swagger.Net.Application;
using Swagger.Net.Dummy;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class SwaggerDocsConfigTests
    {
        [Test]
        public void RootUrl_Test()
        {
            var sc = new SwaggerDocsConfig();
            Assert.DoesNotThrow(() => sc.RootUrl(null));
        }

        [Test]
        public void CustomProvider_Test()
        {
            var sc = new SwaggerDocsConfig();
            Assert.DoesNotThrow(() => sc.CustomProvider(null));
        }

        [Test]
        public void ModelFilter1_Test()
        {
            var sc = new SwaggerDocsConfig();
            Assert.DoesNotThrow(() => sc.ModelFilter(null));
        }

        [Test]
        public void ModelFilter2_Test()
        {
            var sc = new SwaggerDocsConfig();
            Assert.DoesNotThrow(() => sc.ModelFilter<ModFilter>());
        }
    }
}
