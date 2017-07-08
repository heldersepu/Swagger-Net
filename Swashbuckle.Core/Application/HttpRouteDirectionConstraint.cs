using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Swagger.Net.Application
{
    public class HttpRouteDirectionConstraint : IHttpRouteConstraint
    {
        private readonly HttpRouteDirection _allowedDirection;

        public HttpRouteDirectionConstraint(HttpRouteDirection allowedDirection)
        {
            _allowedDirection = allowedDirection;
        }

        public bool Match(
            HttpRequestMessage request,
            IHttpRoute route,
            string parameterName,
            IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            return routeDirection == _allowedDirection;
        }
    }
}
