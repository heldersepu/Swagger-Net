using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Swagger.Net.Swagger;
using System;
using System.Collections.Generic;

namespace Swagger.Net.Tests.CoreUnitTests
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

        [Test]
        public void CreateDefinitionSchema_JsonArrayContract()
        {
            var contract = new JsonArrayContract(typeof(int));
            var mock = new Mock<JsonSerializerSettings>();

            var schema = new SchemaRegistry(mock.Object,
                new Dictionary<Type, Func<Schema>>(),
                new List<ISchemaFilter>(),
                new List<IModelFilter>(),
                true, null, true, true, true);
            Assert.IsNotNull(schema.CreateDefinitionSchema(contract));
        }
    }
}
