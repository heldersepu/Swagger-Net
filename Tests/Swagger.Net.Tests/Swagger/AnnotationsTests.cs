using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using Swagger.Net.Dummy.SwaggerExtensions;
using System.Collections.Generic;

namespace Swagger.Net.Tests.Swagger
{
    [TestFixture]
    public class AnnotationsTests : SwaggerTestBase
    {
        public AnnotationsTests()
            : base("swagger/docs/{apiVersion}")
        {
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultRouteFor<SwaggerAnnotatedController>();

            // Default set-up
            SetUpHandler();
        }

        [Test]
        public void It_assigns_operation_properties_from_swagger_operation_attribute()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);

            var putOperation = swagger["paths"]["/swaggerannotated/{id}"]["put"];

            Assert.AreEqual("UpdateMessage", putOperation["operationId"].ToString());
            Assert.AreEqual(JArray.FromObject(new[] { "messages" }).ToString(), putOperation["tags"].ToString());
            Assert.AreEqual(JArray.FromObject(new[] { "ws" }).ToString(), putOperation["schemes"].ToString());
        }

        [Test]
        public void It_exposes_config_to_post_modify_responses()
        {

            SetUpDefaultRouteFor<ProductsController>();
            SetUpHandler(c => c.OperationFilter<ApplyResponseVendorExtensions>());

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var xProp = swagger["paths"]["/products"]["get"]["responses"]["200"]["x-foo"];

            Assert.IsNotNull(xProp);
            Assert.AreEqual("bar", xProp.ToString());
        }

        [Test]
        public void It_documents_responses_from_swagger_response_attributes_post()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);

            var postResponses = swagger["paths"]["/swaggerannotated"]["post"]["responses"];
            var expected = JObject.FromObject(new Dictionary<string, object>()
                {
                    {
                        "201", new
                        {
                            description = "Created",
                            schema = new
                            {
                                type = "integer",
                                format = "int32"
                            }
                        }
                    },
                    {
                        "400", new
                        {
                            description = "Invalid message",
                            schema = new
                            {
                                additionalProperties = new
                                {
                                    type = "object"
                                },
                                type = "object"
                            }
                        }
                    }
                });
            Assert.AreEqual(expected.ToString(), postResponses.ToString());
        }

        [Test]
        public void It_documents_responses_from_swagger_response_attributes_get()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var getResponses = swagger["paths"]["/swaggerannotated"]["get"]["responses"];
            var expected = JObject.FromObject(new Dictionary<string, object>()
                {
                    {
                        "200", new
                        {
                            description = "OK",
                            schema = new
                            {
                                items = JObject.Parse("{ $ref: \"#/definitions/Message\" }"),
                                xml = JObject.Parse( "{ \"name\": \"Messages\", \"wrapped\": true }" ),
                                type = "array"
                            }
                        }
                    },
                    {
                        "400", new
                        {
                            description = "Bad request"
                        }
                    }
                });
            Assert.AreEqual(expected.ToString(), getResponses.ToString());
        }

        [Test]
        public void It_documents_responses_from_swagger_response_attributes_patch()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var patchResponses = swagger["paths"]["/swaggerannotated"]["patch"]["produces"];
            Assert.AreEqual("[\"image/png\"]", patchResponses.ToString().Strip());
        }

        private dynamic DefaultObj
        {
            get
            {
                return new
                {
                    title = "A message",
                    content = "Some content"
                };
            }
        }

        [Test]
        public void It_supports_per_type_filters_via_swagger_schema_filter_attribute()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);

            var messageExamples = swagger["definitions"]["Message"]["default"];
            var expected = JObject.FromObject(DefaultObj);

            Assert.AreEqual(expected.ToString(), messageExamples.ToString());
        }

        [Test]
        public void It_supports_inherited_per_type_filters_via_swagger_schema_filter_attribute()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);

            var messageExamples = swagger["definitions"]["Message2"]["default"];
            var expected = JObject.FromObject(DefaultObj);

            Assert.AreEqual(expected.ToString(), messageExamples.ToString());
        }

        [Test]
        public void It_supports_per_action_filters_via_swagger_operation_filter_attribute()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);

            var responseExamples = swagger["paths"]["/swaggerannotated/{id}"]["get"]["responses"]["200"]["examples"];
            var expected = JObject.FromObject(new Dictionary<string, object>()
            {
                { "application/json", DefaultObj }
            });

            Assert.AreEqual(expected.ToString(), responseExamples.ToString());
        }

        [Test]
        public void It_has_parameter_descriptions()
        {
            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var description = (string)swagger["paths"]["/swaggerannotated/{id}"]["put"]["parameters"][0]["description"];

            Assert.AreEqual("param description", description);
        }
    }
}
