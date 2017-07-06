using NUnit.Framework;
using Swashbuckle.Swagger.XmlComments;
using System;
using System.IO;
using System.Linq;

namespace Swashbuckle.Tests.CoreUnitTests
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
    }
}
