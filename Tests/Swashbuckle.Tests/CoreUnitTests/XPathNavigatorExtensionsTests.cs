using NUnit.Framework;
using Swashbuckle.Swagger.XmlComments;
using System.IO;
using System.Xml.XPath;

namespace Swashbuckle.Tests.CoreUnitTests
{
    [TestFixture]
    class XPathNavigatorExtensionsTests
    {
        [Test]
        public void XPathNavigator_null()
        {
            XPathNavigator navigator = null;
            string content = navigator.ExtractContent();
            Assert.AreEqual(null, content);
        }

        [Test]
        public void XPathNavigator_reads_empty_doc()
        {
            string xml = "<doc></doc>";
            XPathDocument xmlDoc;
            using (StringReader stream = new StringReader(xml))
            {
                xmlDoc = new XPathDocument(stream);
            }
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            string content = navigator.ExtractContent();
            Assert.AreEqual(xml, content);
        }

        [Test]
        public void XPathNavigator_reads_ConstPattern()
        {
            string xml = "<doc><c>test<display>one</display></c><c>test<display>two</display></c></doc>";
            XPathDocument xmlDoc;
            using (StringReader stream = new StringReader(xml))
            {
                xmlDoc = new XPathDocument(stream);
            }
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            string content = navigator.ExtractContent();
            Assert.AreEqual(xml.Replace("<c>", "").Replace("</c>", ""), content.Strip());
        }

        [Test]
        public void GetConstRefName_null()
        {
            string content = XPathNavigatorExtensions.ConstPattern.Replace("", XPathNavigatorExtensions.GetConstRefName);
            Assert.IsEmpty(content);
        }

        [Test]
        public void GetParamRefName_null()
        {
            string content = XPathNavigatorExtensions.ParamPattern.Replace("", XPathNavigatorExtensions.GetParamRefName);
            Assert.IsEmpty(content);
        }

    }

    internal static class StringExtensions
    {
        internal static string Strip(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace("\n", "").Replace("\r", "").Replace(" ", "");
        }
    }
}
