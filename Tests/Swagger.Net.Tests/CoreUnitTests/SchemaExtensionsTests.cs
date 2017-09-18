using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class SchemaExtensionsTests
    {
        Schema schema
        {
            get
            {
                var schemaRegistry =  new SchemaRegistry(
                    new JsonSerializerSettings(),
                    new SwaggerGeneratorOptions());
                return schemaRegistry.CreateInlineSchema(typeof(object));
            }
        }

        [Test]
        public void WithValidationProperties_Null()
        {
            var mock = new Mock<JsonProperty>();
            Assert.IsNotNull(schema.WithValidationProperties(mock.Object));
        }

        [Test]
        public void PopulateFrom_Null()
        {
            var partSchema = new PartialSchema();
            Schema nullSchema = null;
            Assert.DoesNotThrow(() => partSchema.PopulateFrom(nullSchema));
        }
    }
}
