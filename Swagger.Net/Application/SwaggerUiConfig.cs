using Newtonsoft.Json;
using Swagger.Net.SwaggerUi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Swagger.Net.Application
{
    public class SwaggerUiConfig
    {
        private readonly Dictionary<string, EmbeddedAssetDescriptor> _pathToAssetMap;
        private readonly Dictionary<string, string> _templateParams;
        private readonly Func<HttpRequestMessage, string> _rootUrlResolver;
        private readonly Assembly _thisAssembly;

        public SwaggerUiConfig(IEnumerable<string> discoveryPaths, Func<HttpRequestMessage, string> rootUrlResolver)
        {
            _pathToAssetMap = new Dictionary<string, EmbeddedAssetDescriptor>();

            _templateParams = new Dictionary<string, string>
            {
                { "%(DocumentTitle)", "Swagger UI" },
                { "%(StylesheetIncludes)", "" },
                { "%(DiscoveryUrlSelector)", "false" },
                { "%(DiscoveryPaths)", String.Join("|", discoveryPaths) },
                { "%(BooleanValues)", "true|false" },
                { "%(ValidatorUrl)", "" },
                { "%(CustomScripts)", "" },
                { "%(DocExpansion)", "none" },
                { "%(DefaultModelRendering)", "example" },
                { "%(DefaultModelsExpandDepth)", "0" },
                { "%(DefaultModelExpandDepth)", "0" },
                { "%(UImaxDisplayedTags)", "100" },
                { "%(UIfilter)", "''" },
                { "%(ShowExtensions)", "true" },
                { "%(SupportedSubmitMethods)", "get|put|post|delete|options|head|patch" },
                { "%(OAuth2Enabled)", "false" },
                { "%(OAuth2ClientId)", "" },
                { "%(OAuth2ClientSecret)", "" },
                { "%(OAuth2Realm)", "" },
                { "%(OAuth2AppName)", "" },
                { "%(OAuth2ScopeSeparator)", " " },
                { "%(OAuth2AdditionalQueryStringParams)", "{}" },
                { "%(ApiKeyName)", "api_key" },
                { "%(ApiKeyIn)", "query" }
            };
            _rootUrlResolver = rootUrlResolver;

            _thisAssembly = GetType().Assembly;
            MapPathsForSwaggerUiAssets();

            // Use some custom versions to support config and extensionless paths
            CustomAsset("index", "Swagger.Net.SwaggerUi.CustomAssets.index.html", isTemplate: true);
            CustomAsset("css/typography-css", "Swagger.Net.SwaggerUi.CustomAssets.typography.css");
        }

        public void InjectStylesheet(Assembly resourceAssembly, string resourceName, string media = "screen", bool isTemplate = false)
        {
            var path = "ext/" + resourceName.Replace(".", "-");

            var stringBuilder = new StringBuilder(_templateParams["%(StylesheetIncludes)"]);
            stringBuilder.AppendLine("<link href='" + path + "' media='" + media + "' rel='stylesheet' type='text/css' />");
            _templateParams["%(StylesheetIncludes)"] = stringBuilder.ToString();

            CustomAsset(path, resourceAssembly, resourceName, isTemplate);
        }

        public void BooleanValues(IEnumerable<string> values)
        {
            _templateParams["%(BooleanValues)"] = String.Join("|", values);
        }

        public void DocumentTitle(string title)
        {
            _templateParams["%(DocumentTitle)"] = title;
        }

        public void SetDiscoveryPath(string path)
        {
            _templateParams["%(DiscoveryPaths)"] = path;
        }

        public void SetValidatorUrl(string url)
        {
            _templateParams["%(ValidatorUrl)"] = url;
        }

        public void DisableValidator()
        {
            _templateParams["%(ValidatorUrl)"] = "null";
        }

        public void InjectJavaScript(Assembly resourceAssembly, string resourceName, bool isTemplate = false)
        {
            var path = "ext/" + resourceName.Replace(".", "-");

            var stringBuilder = new StringBuilder(_templateParams["%(CustomScripts)"]);
            if (stringBuilder.Length > 0)
                stringBuilder.Append("|");

            stringBuilder.Append(path);
            _templateParams["%(CustomScripts)"] = stringBuilder.ToString();

            CustomAsset(path, resourceAssembly, resourceName, isTemplate);
        }

        public void DocExpansion(DocExpansion docExpansion)
        {
            _templateParams["%(DocExpansion)"] = docExpansion.ToString().ToLower();
        }

        public void DefaultModelRendering(DefaultModelRender defaultModelRendering)
        {
            _templateParams["%(DefaultModelRendering)"] = defaultModelRendering.ToString().ToLower();
        }

        public void DefaultModelsExpandDepth(int modelsExpansion)
        {
            _templateParams["%(DefaultModelsExpandDepth)"] = modelsExpansion.ToString();
        }

        public void DefaultModelExpandDepth(int modelExpansion)
        {
            _templateParams["%(DefaultModelExpandDepth)"] = modelExpansion.ToString();
        }

        public void UImaxDisplayedTags(int maxDisplayedTags)
        {
            _templateParams["%(UImaxDisplayedTags)"] = maxDisplayedTags.ToString();
        }

        public void UIfilter(string filter)
        {
            _templateParams["%(UIfilter)"] = filter;
        }

        public void ShowExtensions(bool value)
        {
            _templateParams["%(ShowExtensions)"] = value.ToString().ToLower();
        }

        public void SupportedSubmitMethods(params string[] methods)
        {
            _templateParams["%(SupportedSubmitMethods)"] = String.Join("|", methods).ToLower();
        }

        public void CustomAsset(string path, string resourceName, bool isTemplate = false)
        {
            CustomAsset(path, _thisAssembly, resourceName, isTemplate);
        }

        public void CustomAsset(string path, Assembly resourceAssembly, string resourceName, bool isTemplate = false)
        {
            if (path == "index") isTemplate = true;
            _pathToAssetMap[path] = new EmbeddedAssetDescriptor(resourceAssembly, resourceName, isTemplate);
        }

        public void EnableDiscoveryUrlSelector()
        {
            _templateParams["%(DiscoveryUrlSelector)"] = "true";
        }

        public void EnableOAuth2Support(string clientId, string realm, string appName)
        {
            EnableOAuth2Support(clientId, "N/A", realm, appName);
        }

        public void EnableOAuth2Support(
            string clientId,
            string clientSecret,
            string realm,
            string appName,
            string scopeSeperator = " ",
            Dictionary<string, string> additionalQueryStringParams = null)
        {
            _templateParams["%(OAuth2Enabled)"] = "true";
            _templateParams["%(OAuth2ClientId)"] = clientId;
            _templateParams["%(OAuth2ClientSecret)"] = clientSecret;
            _templateParams["%(OAuth2Realm)"] = realm;
            _templateParams["%(OAuth2AppName)"] = appName;
            _templateParams["%(OAuth2ScopeSeparator)"] = scopeSeperator;

            if (additionalQueryStringParams != null)
                _templateParams["%(OAuth2AdditionalQueryStringParams)"] = JsonConvert.SerializeObject(additionalQueryStringParams);
        }

        public void ApiKeySupport(string name, string apiKeyIn) {
            _templateParams["%(ApiKeyName)"] = name;
            _templateParams["%(ApiKeyIn)"] = apiKeyIn;
        }

        internal IAssetProvider GetSwaggerUiProvider()
        {
            return new EmbeddedAssetProvider(_pathToAssetMap, _templateParams);
        }

        internal string GetRootUrl(HttpRequestMessage swaggerRequest)
        {
            return _rootUrlResolver(swaggerRequest);
        }

        private void MapPathsForSwaggerUiAssets()
        {
            foreach (var resourceName in _thisAssembly.GetManifestResourceNames())
            {
                if (resourceName.Contains("Swagger.Net.SwaggerUi.CustomAssets")) continue; // original assets only

                var path = resourceName
                    .Replace("\\", "/")
                    .Replace(".", "-"); // extensionless to avoid RUMMFAR

                _pathToAssetMap[path] = new EmbeddedAssetDescriptor(_thisAssembly, resourceName, path == "index");
            }
        }
    }

    public enum DocExpansion
    {
        None,
        List,
        Full
    }

    public enum DefaultModelRender
    {
        Model,
        Example
    }
}