using Swagger.Net.SwaggerUi;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Swagger.Net.Application
{
    public class SwaggerUiHandler : HttpMessageHandler
    {
        private readonly SwaggerUiConfig _config;

        public SwaggerUiHandler(SwaggerUiConfig config)
        {
            _config = config;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var swaggerUiProvider = _config.GetSwaggerUiProvider();
            var rootUrl = _config.GetRootUrl(request);
            var assetPath = request.GetRouteData().Values["assetPath"].ToString();

            foreach (var query in request.GetQueryNameValuePairs())
            {
                switch (query.Key)
                {
                    case "cssTheme": _config.CssTheme(query.Value); break;
                }
            }

            try
            {
                var webAsset = swaggerUiProvider.GetAsset(rootUrl, assetPath.TrimEnd('/'));
                var content = ContentFor(webAsset);
                return TaskFor(new HttpResponseMessage { Content = content, RequestMessage = request });
            }
            catch (AssetNotFound ex)
            {
                return TaskFor(request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        private HttpContent ContentFor(Asset webAsset)
        {
            int bufferSize = (int)Math.Min(int.MaxValue, webAsset.Stream.Length);
            var content = new StreamContent(webAsset.Stream, bufferSize);
            content.Headers.ContentType = new MediaTypeHeaderValue(webAsset.MediaType);
            return content;
        }

        private Task<HttpResponseMessage> TaskFor(HttpResponseMessage response)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}
