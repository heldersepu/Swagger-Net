using NUnit.Framework;
using Swagger.Net.XmlComments;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace Swagger.Net.Tests.CoreUnitTests
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

        [TestCase(null, "")]
        [TestCase(null, "asdf")]
        [TestCase("<display>abc</display>", "<c><display>abc</display></c>")]
        public void GetConstRefName(string expected, string input)
        {
            Regex rx = XPathNavigatorExtensions.ConstPattern;
            Match m = rx.Match(input);
            string content = XPathNavigatorExtensions.GetConstRefName(m);
            Assert.AreEqual(expected, content);
        }

        [TestCase(null, "")]
        [TestCase(null, "asdf")]
        [TestCase("{<display>abc</display>}", @"<see cref=""T:<display>abc</display>"" />")]
        public void GetParamRefName(string expected, string input)
        {
            Regex rx = XPathNavigatorExtensions.ParamPattern;
            Match m = rx.Match(input);
            string content = XPathNavigatorExtensions.GetParamRefName(m);
            Assert.AreEqual(expected, content);
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
