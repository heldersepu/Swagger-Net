using Moq;
using NUnit.Framework;
using Swashbuckle.Swagger.Annotations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace Swashbuckle.Tests.CoreUnitTests
{
    [TestFixture]
    class ApplySwaggerOperationAttributesTests
    {
        [Test]
        public void GetAsset_null()
        {
            var collSwagger = new Collection<SwaggerOperationAttribute>()
            {
                new SwaggerOperationAttribute()
                {
                    OperationId = null,
                    Tags = null,
                    Schemes = null
                }
            };

            var mock = new Mock<HttpActionDescriptor>();
            mock.Setup(x => x.GetCustomAttributes<SwaggerOperationAttribute>()).Returns(collSwagger);

            var apiDescription = new ApiDescription() { ActionDescriptor = mock.Object };
            var op = new ApplySwaggerOperationAttributes();
            Assert.DoesNotThrow(() => op.Apply(null, null, apiDescription));
        }
    }
}
