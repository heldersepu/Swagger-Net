using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Swashbuckle.Swagger;
using System;

namespace Swashbuckle.Tests.CoreUnitTests
{
    [TestFixture]
    class SchemaRegistryTests
    {

        [Test]
        public void CreateDefinitionSchema_Null()
        {
            var contract = new JsonPrimitiveContract(typeof(int));
            var mock = new Mock<JsonSerializerSettings>();

            var schema = new SchemaRegistry(mock.Object, null, null, null, true, null, true, true, true);
            Assert.Throws<InvalidOperationException>(() => schema.CreateDefinitionSchema(contract));
        }
    }
}
