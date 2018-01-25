using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Swagger.Net.Application
{
    public class SwaggerDocsHandler : HttpMessageHandler
    {
        private static readonly ConcurrentDictionary<string, GeneratedSwaggerDocument> SwaggerDocs = new ConcurrentDictionary<string, GeneratedSwaggerDocument>();
        private readonly SwaggerDocsConfig _config;

        public SwaggerDocsHandler(SwaggerDocsConfig config)
        {
            _config = config;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var apiVersion = GetRequestApiVersion(request);
                SwaggerDocs.TryGetValue(apiVersion, out var generatedSwagger);

                if (request.Headers.IfModifiedSince != null && generatedSwagger?.LastModified <= request.Headers.IfModifiedSince)
                {
                    return Task.FromResult(request.CreateResponse(HttpStatusCode.NotModified));
                }

                var response = new HttpResponseMessage { Content = GetContent(request) };
                string accessControlAllowOrigin = _config.GetAccessControlAllowOrigin();
                if (!string.IsNullOrEmpty(accessControlAllowOrigin))
                {
                    response.Headers.Add("Access-Control-Allow-Origin", accessControlAllowOrigin);
                }

                return Task.FromResult(response);
            }
            catch (UnknownApiVersion ex)
            {
                return Task.FromResult(request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        private static string GetRequestApiVersion(HttpRequestMessage request)
        {
            var apiVersion = request.GetRouteData().Values["apiVersion"].ToString();
            return apiVersion;
        }

        private HttpContent GetContent(HttpRequestMessage request)
        {
            var apiVersion = GetRequestApiVersion(request);

            GeneratedSwaggerDocument swaggerDoc;
            if (_config.NoCachingSwaggerDoc())
            {
                // always force a regeneration
                swaggerDoc = SwaggerDocs
                    .AddOrUpdate(
                        apiVersion,
                        _ => Generate(request, apiVersion),
                        (_, __) => Generate(request, apiVersion));
            }
            else
            {
                // generate or return the cached copy
                swaggerDoc = SwaggerDocs
                    .GetOrAdd(
                        apiVersion,
                        _ => Generate(request, apiVersion));
            }

            return ContentFor(request, swaggerDoc);
        }

        private GeneratedSwaggerDocument Generate(HttpRequestMessage request, string apiVersion)
        {
            var swaggerProvider = _config.GetSwaggerProvider(request);
            var rootUrl = _config.GetRootUrl(request);

            var swaggerDoc = swaggerProvider.GetSwagger(rootUrl, apiVersion.ToUpper());

            return new GeneratedSwaggerDocument
            {
                SwaggerDocument = swaggerDoc,
                LastModified = DateTimeOffset.UtcNow
            };
        }

        private HttpContent ContentFor(HttpRequestMessage request, GeneratedSwaggerDocument swaggerDoc)
        {
            var negotiator = request.GetConfiguration().Services.GetContentNegotiator();
            var result = negotiator.Negotiate(typeof(SwaggerDocument), request, GetSupportedSwaggerFormatters());

            var content = new ObjectContent(typeof(SwaggerDocument), swaggerDoc.SwaggerDocument, result.Formatter, result.MediaType);
            content.Headers.LastModified = swaggerDoc.LastModified;
            return content;
        }

        private IEnumerable<MediaTypeFormatter> GetSupportedSwaggerFormatters()
        {
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = _config.GetFormatting(),
                    Converters = new[] { new VendorExtensionsConverter() }
                }
            };
            // NOTE: The custom converter would not be necessary in Newtonsoft.Json >= 5.0.5 as JsonExtensionData
            // provides similar functionality. But, need to stick with older version for WebApi 5.0.0 compatibility
            return new[] { jsonFormatter };
        }

        private class GeneratedSwaggerDocument
        {
            public SwaggerDocument SwaggerDocument { get; set; }

            public DateTimeOffset LastModified { get; set; }
        }
    }
}
