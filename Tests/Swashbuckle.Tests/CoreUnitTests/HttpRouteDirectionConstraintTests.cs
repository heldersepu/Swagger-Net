using NUnit.Framework;
using Swashbuckle.Application;
using System.Web.Http.Routing;

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
