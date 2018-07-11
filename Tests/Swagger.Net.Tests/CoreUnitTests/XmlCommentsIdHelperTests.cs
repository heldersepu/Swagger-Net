using NUnit.Framework;
using Swagger.Net.Dummy.Controllers;
using Swagger.Net.XmlComments;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class XmlCommentsIdHelperTests
    {
        const string CTRLR = "M:Swagger.Net.Dummy.Controllers.";

        [Test]
        public void Method_from_inherited_action_post()
        {
            var methodInfo = typeof(BaseChildController).GetMethod("Post");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "BaseController`1.Post(System.String)", comment);
        }

        [Test]
        public void Method_from_inherited_action_put()
        {
            var methodInfo = typeof(BaseChildController).GetMethod("Put");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "BaseController`1.Put(`0,System.String)", comment);
        }

        [Test]
        public void Method_from_generic_action_post()
        {
            var methodInfo = typeof(BlobController).GetMethod("Post");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "Blob`1.Post(System.Int32)", comment);
        }

        [Test]
        public void Method_from_generic_action_get()
        {
            var methodInfo = typeof(BlobController).GetMethod("Get");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "Blob`1.Get(System.Nullable{System.Int32})", comment);
        }

        [Test]
        public void Method_from_generic_action_put()
        {
            var methodInfo = typeof(BlobController).GetMethod("Put");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "Blob`1.Put(Swagger.Net.Dummy.Controllers.AnotherFoo{`0})", comment);
        }

        [Test]
        public void Method_from_generic_action_patch()
        {
            var methodInfo = typeof(BlobControllerThree).GetMethod("Patch");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "Blob`3.Patch(Swagger.Net.Dummy.Controllers.AnotherFoo{`2},`0)", comment);
        }

        [Test]
        public void Method_from_NestedEnum_action_get()
        {
            var methodInfo = typeof(NestedEnumController).GetMethod("Get");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "NestedEnum`1.Get(System.Nullable{Swagger.Net.Dummy.Controllers.NestedEnum{`0}.Giorno})", comment);
        }

        [Test]
        public void Method_from_NestedEnum_action_put()
        {
            var methodInfo = typeof(NestedEnumController).GetMethod("Put");
            var comment = methodInfo.GetCommentIdForMethod();
            Assert.AreEqual(CTRLR + "NestedEnum`1.Put(Swagger.Net.Dummy.Controllers.NestedEnum{`0}.Giorno)", comment);
        }
    }
}
