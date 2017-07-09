using Moq;
using NUnit.Framework;
using Swagger.Net.Swagger;
using Swagger.Net.XmlComments;
using System;
using System.IO;
using System.Linq;
using System.Web.Http.Description;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class ApplyXmlActionCommentsTests
    {
        [Test]
        public void ApplyXmlActionComments_Empty()
        {
            Assert.Throws<ArgumentException>(() => new ApplyXmlActionComments(""));
        }

        [Test]
        public void ApplyXmlActionComments_Test()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string xmlFile = Directory.GetFiles(directory, "*.XML", SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(xmlFile))
            {
                Assert.Inconclusive();
            }
            else
            {
                Assert.DoesNotThrow(() => new ApplyXmlActionComments(xmlFile));
            }
        }

        [Test]
        public void reflectedActionDescriptor_Test()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string xmlFile = Directory.GetFiles(directory, "*.XML", SearchOption.AllDirectories).FirstOrDefault();
            var action = new ApplyXmlActionComments(xmlFile);

            var mock = new Mock<ApiDescription>();
            Assert.DoesNotThrow(() => action.Apply(null, null, mock.Object));
        }

        [Test]
        public void ApplyParamComments_Test()
        {
            var mock = new Mock<Operation>();
            Assert.DoesNotThrow(() => ApplyXmlActionComments.ApplyParamComments(mock.Object, null, null));
        }
    }
}
