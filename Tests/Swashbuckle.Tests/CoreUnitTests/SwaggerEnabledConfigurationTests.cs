using NUnit.Framework;
using Swagger.Net.Application;
using System.Collections.Generic;
using System.Web.Http;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class SwaggerEnabledConfigurationTests
    {
        private SwaggerEnabledConfiguration swag
        {
            get
            {
                return new SwaggerEnabledConfiguration(
                    new HttpConfiguration(),
                    null,
                    new List<string>()
                );
            }
        }

        [Test]
        public void EnableSwaggerUi_Test()
        {
            Assert.DoesNotThrow(() => swag.EnableSwaggerUi("test", c => { }));
        }

        [Test]
        public void EnableSwaggerUi_Null()
        {
            Assert.DoesNotThrow(() => swag.EnableSwaggerUi("test", null));
        }
    }
}
