using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class SchemaRegistryTests
    {
        [Test]
        public void CreateDictionarySchema_Null()
        {
            var mockContract = new Mock<JsonDictionaryContract>(typeof(Dictionary<int,int>));
            //mockContract.Setup(x => x.DictionaryKeyType).Returns((Type)null);
            var mockJsonSerializer = new Mock<JsonSerializerSettings>();

            bool hasError = false;
            try
            {
                var schema = new SchemaRegistry(mockJsonSerializer.Object, null);
                schema.CreateDictionarySchema(mockContract.Object);
            }
            catch 
            {
                hasError = true;
            }
            Assert.IsTrue(hasError);
        }

        [Test]
        public void CreateDefinitionSchema_Null()
        {
            var contract = new JsonPrimitiveContract(typeof(int));
            var mock = new Mock<JsonSerializerSettings>();

            var schema = new SchemaRegistry(mock.Object, new SwaggerGeneratorOptions());
            Assert.Throws<InvalidOperationException>(() => schema.CreateDefinitionSchema(contract));
        }

        [Test]
        public void CreateDefinitionSchema_JsonArrayContract()
        {
            var contract = new JsonArrayContract(typeof(int));
            var mock = new Mock<JsonSerializerSettings>();

            var schema = new SchemaRegistry(mock.Object, new SwaggerGeneratorOptions());
            Assert.IsNotNull(schema.CreateDefinitionSchema(contract));
        }
    }


}
