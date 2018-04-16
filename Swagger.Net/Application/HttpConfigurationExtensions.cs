using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Swagger.Net.Application
{
    public static class HttpConfigurationExtensions
    {
        private static readonly string DefaultRouteTemplate = "swagger/docs/{apiVersion}";

        public static SwaggerEnabledConfiguration EnableSwagger(
            this HttpConfiguration httpConfig,
            Action<SwaggerDocsConfig> configure = null)
        {
            return EnableSwagger(httpConfig, DefaultRouteTemplate, configure);
        }

        public static SwaggerEnabledConfiguration EnableSwagger(
            this HttpConfiguration httpConfig,
            string routeTemplate,
            Action<SwaggerDocsConfig> configure = null)
        {
            var config = new SwaggerDocsConfig();
            if (configure != null) configure(config);

            httpConfig.Routes.MapHttpRoute(
                name: "swagger_docs" + routeTemplate,
                routeTemplate: routeTemplate,
                defaults: null,
                constraints: new { apiVersion = @".+" },
                handler: new SwaggerDocsHandler(config)
            );

            return new SwaggerEnabledConfiguration(httpConfig, config, routeTemplate);
        }

        internal static JsonSerializerSettings SerializerSettingsOrDefault(this HttpConfiguration httpConfig)
        {
            var formatter = httpConfig.Formatters.JsonFormatter;
            if (formatter != null)
                return formatter.SerializerSettings;

            return new JsonSerializerSettings();
        }
    }

    public class SwaggerEnabledConfiguration
    {
        private static readonly string DefaultRouteTemplate = "swagger/ui/{*assetPath}";

        private readonly HttpConfiguration _httpConfig;
        private readonly SwaggerDocsConfig _config;
        private readonly string _route;

        public SwaggerEnabledConfiguration(HttpConfiguration httpConfig, SwaggerDocsConfig config, string route)
        {
            _httpConfig = httpConfig;
            _config = config;
            _route = route;
        }

        public void EnableSwaggerUi(Action<SwaggerUiConfig> configure = null)
        {
            EnableSwaggerUi(DefaultRouteTemplate, configure);
        }

        public void EnableSwaggerUi(string routeTemplate, Action<SwaggerUiConfig> configure = null)
        {
            var config = new SwaggerUiConfig(_config.DiscoveryPaths(_route), _config.GetRootUrl);
            configure?.Invoke(config);

            _httpConfig.Routes.MapHttpRoute(
                name: "swagger_ui" + routeTemplate,
                routeTemplate: routeTemplate,
                defaults: null,
                constraints: new { assetPath = @".+" },
                handler: new SwaggerUiHandler(config)
            );

            if (routeTemplate == DefaultRouteTemplate)
            {
                _httpConfig.Routes.MapHttpRoute(
                    name: "swagger_ui_shortcut",
                    routeTemplate: "swagger",
                    defaults: null,
                    constraints: new { uriResolution = new HttpRouteDirectionConstraint(HttpRouteDirection.UriResolution) },
                    handler: new RedirectHandler(_config.GetRootUrl, "swagger/ui/index"));
            }
        }
    }
}