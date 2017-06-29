using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Collections.Generic;
using Swashbuckle.Swagger;
using System.Net;

namespace Swashbuckle.Application
{
    public class SwaggerDocsHandler : HttpMessageHandler
    {
        private static SwaggerDocument swaggerDoc = null;
        private readonly SwaggerDocsConfig _config;

        public SwaggerDocsHandler(SwaggerDocsConfig config)
        {
            _config = config;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new HttpResponseMessage { Content = GetContent(request) };
                string accessControlAllowOrigin = _config.GetAccessControlAllowOrigin();
                if (!string.IsNullOrEmpty(accessControlAllowOrigin))
                {
                    response.Headers.Add("Access-Control-Allow-Origin", accessControlAllowOrigin);
                }
                return TaskFor(response);
            }
            catch (UnknownApiVersion ex)
            {
                return TaskFor(request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        private HttpContent GetContent(HttpRequestMessage request)
        {
            if (_config.NoCachingSwaggerDoc() || swaggerDoc == null)
            {
                var swaggerProvider = _config.GetSwaggerProvider(request);
                var rootUrl = _config.GetRootUrl(request);
                var apiVersion = request.GetRouteData().Values["apiVersion"].ToString();

                swaggerDoc = swaggerProvider.GetSwagger(rootUrl, apiVersion.ToUpper());
            }
            return ContentFor(request, swaggerDoc);
        }

        private HttpContent ContentFor(HttpRequestMessage request, SwaggerDocument swaggerDoc)
        {
            var negotiator = request.GetConfiguration().Services.GetContentNegotiator();
            var result = negotiator.Negotiate(typeof(SwaggerDocument), request, GetSupportedSwaggerFormatters());

            return new ObjectContent(typeof(SwaggerDocument), swaggerDoc, result.Formatter, result.MediaType);
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
            // NOTE: The custom converter would not be neccessary in Newtonsoft.Json >= 5.0.5 as JsonExtensionData
            // provides similar functionality. But, need to stick with older version for WebApi 5.0.0 compatibility
            return new[] { jsonFormatter };
        }

        private Task<HttpResponseMessage> TaskFor(HttpResponseMessage response)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}
