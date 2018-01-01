using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;

namespace Swagger.Net.Tests.Swagger
{
    [TestFixture]
    public class SchemaArraysTests : SwaggerTestBase
    {
        public SchemaArraysTests() : base("swagger/docs/{apiVersion}") { }

        [SetUp]
        public void SetUp()
        {
            // Default set-up
            SetUpHandler();
        }

        [TestCase("string", "type")]
        [TestCase("integer", "type")]
        [TestCase("location", "$ref")]
        public void It_handles_arrays(string type, string item)
        {
            SetUpAttributeRoutesFrom(typeof(ArraysTestController).Assembly);

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var get = swagger["paths"]["/list_" + type]["get"];
            var param = get["parameters"][0];
            var resp_schema = get["responses"]["200"]["schema"];

            Assert.AreEqual("p", param["name"].ToString());
            Assert.AreEqual("array", param["type"].ToString());

            Assert.AreEqual("array", resp_schema["type"].ToString());
            Assert.AreEqual("array", resp_schema["type"].ToString());
            var resp_item = resp_schema["items"][item].ToString();
            Assert.IsTrue(resp_item.ToLower().EndsWith(type), resp_item);
        }
    }
}
