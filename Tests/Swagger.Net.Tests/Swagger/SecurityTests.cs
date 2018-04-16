using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using Swagger.Net.Tests.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace Swagger.Net.Tests.SwaggerFilters
{
    [TestFixture]
    public class SecurityTests : SwaggerTestBase
    {
        public SecurityTests()
            : base("swagger/docs/{apiVersion}")
        {
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultRouteFor<ProtectedResourcesController>();
        }

        [Test]
        public void It_exposes_config_to_define_a_basic_auth_scheme_for_the_api()
        {
            SetUpHandler(c =>
                {
                    c.BasicAuth("basic")
                        .Description("Basic HTTP Authentication");
                });

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var securityDefinitions = swagger["securityDefinitions"];
            var expected = JObject.FromObject(new
                {
                    basic = new
                    {
                        type = "basic",
                        description = "Basic HTTP Authentication"
                    }
                });

            Assert.AreEqual(expected.ToString(), securityDefinitions.ToString());
        }

        [Test]
        public void It_exposes_config_to_define_an_api_key_auth_scheme_for_the_api()
        {
            SetUpHandler(c =>
                {
                    c.ApiKey("apiKey", "header", "API Key Authentication");
                    c.ApiKey("appId", "header", "APP ID Authentication");
                });

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var securityDefinitions = swagger["securityDefinitions"];
            var expected = JObject.FromObject(new
            {
                apiKey = new
                {
                    type = "apiKey",
                    description = "API Key Authentication",
                    name = "apiKey",
                    @in = "header",
                },
                appId = new
                {
                    type = "apiKey",
                    description = "APP ID Authentication",
                    name = "appId",
                    @in = "header",
                }
            });

            Assert.AreEqual(expected.ToString(), securityDefinitions.ToString());
        }

        [Test]
        public void It_exposes_config_to_define_an_api_key_and_basic_auth_schemes()
        {
            SetUpHandler(c =>
            {
                c.ApiKey("apiKey", "header", "API Key Auth");
                c.BasicAuth("BasicAuth");
            });

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var securityDefinitions = swagger["securityDefinitions"];
            var expected = JObject.FromObject(new
            {
                apiKey = new
                {
                    type = "apiKey",
                    description = "API Key Auth",
                    name = "apiKey",
                    @in = "header",
                },
                BasicAuth = new
                {
                    type = "basic"
                }
            });

            Assert.AreEqual(expected.ToString(), securityDefinitions.ToString());
        }

        [Test]
        public void It_exposes_config_to_define_an_api_key_auth_scheme_for_the_api_dups()
        {
            SetUpHandler(c =>
            {
                c.ApiKey("apiKey", "header", "API Key Authentication");
                c.OperationFilter<AssignApiKeySecurityRequirements>();
            });

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var securityDefinitions = swagger["securityDefinitions"];
            var expected = JObject.FromObject(new
            {
                apiKey = new
                {
                    type = "apiKey",
                    description = "API Key Authentication",
                    name = "apiKey",
                    @in = "header",
                }
            });

            Assert.AreEqual(expected.ToString(), securityDefinitions.ToString());
        }

        [Test]
        public void It_exposes_config_to_define_oauth2_flows_for_the_api()
        {
            SetUpHandler(c =>
                {
                    c.OAuth2("oauth2")
                        .Description("OAuth2 Authorization Code Grant")
                        .Flow("accessCode")
                        .AuthorizationUrl("https://tempuri.org/auth")
                        .TokenUrl("https://tempuri.org/token")
                        .Scopes(s =>
                        {
                            s.Add("read", "Read access to protected resources");
                            s.Add("write", "Write access to protected resources");
                        });
                });

            var swagger = GetContent<JObject>(TEMP_URI.DOCS);
            var securityDefinitions = swagger["securityDefinitions"];
            var expected = JObject.FromObject(new
                {
                    oauth2 = new
                    {
                        type = "oauth2",
                        description = "OAuth2 Authorization Code Grant",
                        flow = "accessCode",
                        authorizationUrl = "https://tempuri.org/auth",
                        tokenUrl = "https://tempuri.org/token",
                        scopes = new
                        {
                            read = "Read access to protected resources",
                            write = "Write access to protected resources"
                        },
                    }
                });

            Assert.AreEqual(expected.ToString(), securityDefinitions.ToString());
        }
    }

    public class AssignApiKeySecurityRequirements : IOperationFilter
    {
        public void Apply(Operation o, SchemaRegistry s, ApiDescription a)
        {
            if (o.security == null)
                o.security = new List<IDictionary<string, IEnumerable<string>>>();

            var SecRequirements = new Dictionary<string, IEnumerable<string>>
                {
                    { "apiKey", new List<string>() }
                };
            o.security.Add(SecRequirements);
        }
    }
}