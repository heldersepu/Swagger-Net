using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using Swagger.Net.XmlComments;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class XmlCommentsIdHelperTests
    {
        [Test]
        public void Method_from_inherited_action_post()
        {
            var methodInfo = typeof(BaseChildController).GetMethod("Post");
            var comment = XmlCommentsIdHelper.GetCommentIdForMethod(methodInfo);
            Assert.AreEqual("M:Swagger.Net.Dummy.Controllers.BaseController`1.Post(System.String)", comment);
        }

        [Test]
        public void Method_from_inherited_action_put()
        {
            var methodInfo = typeof(BaseChildController).GetMethod("Put");
            var comment = XmlCommentsIdHelper.GetCommentIdForMethod(methodInfo);
            Assert.AreEqual("M:Swagger.Net.Dummy.Controllers.BaseController`1.Put(`0,System.String)", comment);
        }
    }
}
