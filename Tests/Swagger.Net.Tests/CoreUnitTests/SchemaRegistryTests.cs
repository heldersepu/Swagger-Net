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
        public void SchemaRegistry_Null1()
        {
            var mock = new Mock<JsonSerializerSettings>();
            Assert.Throws<NullReferenceException>(() => new SchemaRegistry(mock.Object, null));
        }

        [Test]
        public void SchemaRegistry_Null2()
        {
            Assert.Throws<NullReferenceException>(() => new SchemaRegistry(null, null));
        }

        [Test]
        public void CreateDictionarySchema_Empty()
        {
            var mock = new Mock<JsonDictionaryContract>(typeof(Dictionary<int,int>));
            var mockJsonSerializer = new Mock<JsonSerializerSettings>();

            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mockJsonSerializer.Object, opt);
            Assert.DoesNotThrow(() => schema.CreateDictionarySchema(mock.Object));
        }

        [Test]
        public void CreateDictionarySchema_Null()
        {
            var mock = new Mock<JsonDictionaryContract>(typeof(Dictionary<int, int>));
            //mock.Setup(x => x.DictionaryKeyType).Returns(null);
            var mockJsonSerializer = new Mock<JsonSerializerSettings>();

            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mockJsonSerializer.Object, opt);
            Assert.DoesNotThrow(() => schema.CreateDictionarySchema(mock.Object));
        }

        [Test]
        public void CreateObjectSchema_Null()
        {
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mock.Object, opt);
            Assert.Throws<NullReferenceException>(() => schema.CreateObjectSchema(null));
        }

        [Test]
        public void CreateObjectSchema_addXmlName()
        {
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mock.Object, opt);
            var c = new JsonObjectContract(typeof(int));
            var obj = schema.CreateObjectSchema(c, false);
            Assert.IsNull(obj.xml);
            obj = schema.CreateObjectSchema(c, true);
            Assert.IsNotNull(obj.xml);
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
