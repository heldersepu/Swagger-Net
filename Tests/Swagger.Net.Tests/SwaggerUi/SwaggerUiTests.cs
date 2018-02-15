using NUnit.Framework;
using Swagger.Net.Application;
using Swagger.Net.Dummy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Swagger.Net.Tests.SwaggerUi
{
    [TestFixture]
    public class SwaggerUiTests : HttpMessageHandlerTestBase<SwaggerUiHandler>
    {
        public SwaggerUiTests() : base("swagger/ui/{*assetPath}") { }

        [SetUp]
        public void SetUp()
        {
            // Default set-up
            SetUpHandler();
        }

        [Test]
        public void It_serves_the_embedded_swagger_ui()
        {
            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("discoveryPaths: arrayFrom('swagger/docs/v1')", content);
            StringAssert.Contains("swagger-ui-bundle", content);
        }

        [Test]
        public void It_serves_the_embedded_swagger_ui_with_ending_slash()
        {
            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("swagger-ui-bundle", content);
        }

        [Test]
        public void It_exposes_config_to_inject_custom_stylesheets()
        {
            SetUpHandler(c =>
                {
                    var assembly = typeof(SwaggerConfig).Assembly;
                    c.EnableDiscoveryUrlSelector();
                    c.InjectStylesheet(assembly, "Swagger.Net.Dummy.SwaggerExtensions.testStyles1.css");
                    c.InjectStylesheet(assembly, "Swagger.Net.Dummy.SwaggerExtensions.testStyles2.css", "print");
                });


            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains(
                "<link href='ext/Swagger-Net-Dummy-SwaggerExtensions-testStyles1-css' media='screen' rel='stylesheet' type='text/css' />\r\n" +
                "<link href='ext/Swagger-Net-Dummy-SwaggerExtensions-testStyles2-css' media='print' rel='stylesheet' type='text/css' />",
                content);

            content = GetContentAsString("http://tempuri.org/swagger/ui/ext/Swagger-Net-Dummy-SwaggerExtensions-testStyles1-css");
            StringAssert.StartsWith("h1", content);

            content = GetContentAsString("http://tempuri.org/swagger/ui/ext/Swagger-Net-Dummy-SwaggerExtensions-testStyles2-css");
            StringAssert.StartsWith("h2", content);
        }

        [Test]
        public void It_exposes_config_to_set_the_page_title()
        {
            string customTitle = string.Format("TEST_TITLE_{0}", Guid.NewGuid());
            SetUpHandler(c => { c.DocumentTitle(customTitle); });

            var content = GetContentAsString(TEMP_URI.INDEX);
            StringAssert.Contains(customTitle, content);
        }

        [Test]
        public void It_exposes_config_for_swagger_ui_settings()
        {
            SetUpHandler(c =>
                {
                    c.DocExpansion(DocExpansion.Full);
                    c.DefaultModelRendering(DefaultModelRender.Model);
                    c.DefaultModelExpandDepth(1);
                    c.BooleanValues(new[] { "1", "0" });
                    c.ShowExtensions(true);
                    c.SupportedSubmitMethods("GET", "HEAD");
                    c.EnableOAuth2Support("test-client-id", "test-realm", "Swagger UI");
                });

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("docExpansion: 'full'", content);
            StringAssert.Contains("booleanValues: arrayFrom('1|0')", content);
            StringAssert.Contains("supportedSubmitMethods: arrayFrom('get|head')", content);
            StringAssert.Contains("showExtensions: ('true' == 'true')", content);
        }

        [Test]
        public void It_exposes_config_for_swagger_ui_outh2_settings()
        {
            SetUpHandler(c =>
                {
                    c.EnableOAuth2Support(
                        "test-client-id",
                        "test-client-secret",
                        "test-realm",
                        "Swagger UI",
                        " ",
                        new Dictionary<string, string> { { "TestHeader", "TestValue" } });
                });

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("oAuth2Enabled: ('true' == 'true')", content);
            StringAssert.Contains("oAuth2ClientId: 'test-client-id'", content);
            StringAssert.Contains("oAuth2ClientSecret: 'test-client-secret'", content);
            StringAssert.Contains("oAuth2Realm: 'test-realm'", content);
            StringAssert.Contains("oAuth2AppName: 'Swagger UI'", content);
            StringAssert.Contains("OAuth2ScopeSeparator: ' '", content);
            StringAssert.Contains("oAuth2AdditionalQueryStringParams: JSON.parse('{\"TestHeader\":\"TestValue\"}')", content);
        }

        [Test]
        public void It_exposes_config_to_inject_custom_javascripts()
        {
            SetUpHandler(c =>
                {
                    var assembly = typeof(SwaggerConfig).Assembly;
                    c.InjectJavaScript(assembly, "Swagger.Net.Dummy.SwaggerExtensions.testScript1.js");
                    c.InjectJavaScript(assembly, "Swagger.Net.Dummy.SwaggerExtensions.testScript2.js");
                });

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains(
                "customScripts: " +
                "arrayFrom('ext/Swagger-Net-Dummy-SwaggerExtensions-testScript1-js|" +
                "ext/Swagger-Net-Dummy-SwaggerExtensions-testScript2-js')",
                content);

            content = GetContentAsString("http://tempuri.org/swagger/ui/ext/Swagger-Net-Dummy-SwaggerExtensions-testScript1-js");
            StringAssert.StartsWith("var str1", content);

            content = GetContentAsString("http://tempuri.org/swagger/ui/ext/Swagger-Net-Dummy-SwaggerExtensions-testScript2-js");
            StringAssert.StartsWith("var str2", content);
        }

        [Test]
        public void It_exposes_config_to_serve_custom_assets()
        {
            SetUpHandler(c =>
                {
                    var assembly = typeof(SwaggerConfig).Assembly;
                    c.CustomAsset("index", assembly, "Swagger.Net.Dummy.SwaggerExtensions.myIndex.html");
                });

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("My Index", content);
        }

        [Test]
        public void It_exposes_config_to_set_validator_url()
        {
            SetUpHandler(c => c.SetValidatorUrl("http://my-validator.url"));

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("validatorUrl: stringOrNullFrom('http://my-validator.url')", content);
        }

        [Test]
        public void It_exposes_config_to_disable_validator()
        {
            SetUpHandler(c => c.DisableValidator());

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("validatorUrl: stringOrNullFrom('null')", content);
        }

        [Test]
        public void It_errors_on_asset_not_found_and_returns_status_not_found()
        {
            var response = Get("http://tempuri.org/swagger/ui/ext/foobar");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestCase("http://tempuri.org/swagger/ui/swagger-ui-bundle-js", "text/javascript")]
        [TestCase("http://tempuri.org/swagger/ui/css/typography-css",   "text/css")]
        public void It_returns_correct_asset_mime_type(string resourceUri, string mediaType)
        {
            var response = Get(resourceUri);
            var resMediaType = response.Content.Headers.ContentType.MediaType;

            Debug.WriteLine(string.Format("[{0}] {1} => {2}", response.StatusCode, resourceUri, resMediaType));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(mediaType, resMediaType);
        }

        private void SetUpHandler(Action<SwaggerUiConfig> configure = null)
        {
            var swaggerUiConfig = new SwaggerUiConfig(new[] { "swagger/docs/v1" }, SwaggerDocsConfig.DefaultRootUrlResolver);

            configure?.Invoke(swaggerUiConfig);

            Handler = new SwaggerUiHandler(swaggerUiConfig);
        }

        [Test]
        public void It_exposes_config_for_UIfilter()
        {
            SetUpHandler(c => { c.UIfilter("'Xml'"); });

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("filter: 'Xml'", content);
        }

        [Test]
        public void It_exposes_config_for_UImaxDisplayedTags()
        {
            SetUpHandler(c => { c.UImaxDisplayedTags(2); });

            var content = GetContentAsString(TEMP_URI.INDEX);

            StringAssert.Contains("maxDisplayedTags: 2", content);
        }
    }
}