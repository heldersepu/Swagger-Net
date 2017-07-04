using NUnit.Framework;
using Swashbuckle.Application;
using Swashbuckle.Swagger.XmlComments;
using System.IO;
using System.Web.Http.Routing;
using System.Xml.XPath;

namespace Swashbuckle.Tests.CoreUnitTests
{
    [TestFixture]
    class HttpRouteDirectionConstraintTests
    {
        [Test]
        public void It_returns_match()
        {
            HttpRouteDirection direction =  HttpRouteDirection.UriResolution;
            HttpRouteDirectionConstraint route = new HttpRouteDirectionConstraint(direction);
            Assert.IsTrue(route.Match(null,null,"", null, direction));
        }
    }
}
