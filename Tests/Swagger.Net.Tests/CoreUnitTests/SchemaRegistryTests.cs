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
        public void GetOrRegister_withoutTypeName()
        {
            string SchemaIdSelector(Type arg) => arg.FullName;
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions(schemaIdSelector: SchemaIdSelector);
            var schema = new SchemaRegistry(mock.Object, opt);
            var modelType = new {Property1 = "Property1"}.GetType();

            schema.GetOrRegister(modelType);

            Assert.That(schema.Definitions, Is.Not.Null.And.ContainKey(SchemaIdSelector(modelType)));
            Assert.That(schema.Definitions[SchemaIdSelector(modelType)].properties, Is.Not.Null.And.ContainKey("Property1"));
        }

        [Test]
        public void GetOrRegister_withTypeName()
        {
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mock.Object, opt);
            var model = new {Property1 = "Property1"};
            const string typeName = "testTypeName";

            schema.GetOrRegister(model.GetType(), typeName);

            Assert.That(schema.Definitions, Is.Not.Null.And.ContainKey(typeName));
            Assert.That(schema.Definitions[typeName].properties, Is.Not.Null.And.ContainKey("Property1"));
        }

        [Test]
        public void GetOrRegister_registerMultiple_SameType_SameName_RegistersOne()
        {
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mock.Object, opt);
            var model = new {Property1 = "Property1"};
            const string testType = "testTypeName";

            schema.GetOrRegister(model.GetType(), testType);
            schema.GetOrRegister(model.GetType(), testType);

            Assert.That(schema.Definitions, Is.Not.Null.And.Count.EqualTo(1));
            Assert.That(schema.Definitions, Contains.Key(testType));
            Assert.That(schema.Definitions[testType].properties, Is.Not.Null.And.ContainKey("Property1"));
        }

        [Test]
        public void GetOrRegister_registerMultiple_SameType_DifferentName_RegistersTwo()
        {
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mock.Object, opt);
            var model = new {Property1 = "Property1"};
            const string testType1 = "testTypeName1";
            const string testType2 = "testTypeName2";

            schema.GetOrRegister(model.GetType(), testType1);
            schema.GetOrRegister(model.GetType(), testType2);

            Assert.That(schema.Definitions, Is.Not.Null.And.Count.EqualTo(2));
            Assert.That(schema.Definitions, Contains.Key(testType1).And.ContainKey(testType2));
            Assert.That(schema.Definitions[testType1].properties, Is.Not.Null.And.ContainKey("Property1"));
            Assert.That(schema.Definitions[testType2].properties, Is.Not.Null.And.ContainKey("Property1"));
        }

        [Test]
        public void GetOrRegister_registerMultiple_DifferentType_SameName_ThrowsError()
        {
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();
            var schema = new SchemaRegistry(mock.Object, opt);
            var model1 = new {Property1 = "Property1"};
            var model2 = new {Property2 = "Property2"};
            const string testType = "testTypeName";

            schema.GetOrRegister(model1.GetType(), testType);
            Assert.Throws<InvalidOperationException>(() => schema.GetOrRegister(model2.GetType(), testType));
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

        [Test]
        public void CreateEnumSchema_null()
        {
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();

            var schema = new SchemaRegistry(mock.Object, opt);
            Assert.Throws<NullReferenceException>(() => schema.CreateEnumSchema(null, null));
        }

        [Test]
        public void CreateEnumSchema_empty()
        {
            var contract = new JsonPrimitiveContract(typeof(int));
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions();

            var schema = new SchemaRegistry(mock.Object, opt);
            var enu = schema.CreateEnumSchema(contract, typeof(int));
            Assert.IsNotNull(enu);
        }

        [Test]
        public void CreateEnumSchema_camelCase()
        {
            var contract = new JsonPrimitiveContract(typeof(int));
            var mock = new Mock<JsonSerializerSettings>();
            var opt = new SwaggerGeneratorOptions(
                describeAllEnumsAsStrings: true,
                describeStringEnumsInCamelCase: true
            );

            var schema = new SchemaRegistry(mock.Object, opt);
            var enu = schema.CreateEnumSchema(contract, typeof(int));
            Assert.IsNotNull(enu);
        }
    }
}
