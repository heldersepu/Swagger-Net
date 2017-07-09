using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace Swagger.Net.XmlComments
{
    public static class XPathNavigatorExtensions
    {
        public static Regex ParamPattern = new Regex(@"<(see|paramref) (name|cref)=""([TPF]{1}:)?(?<display>.+?)"" />");
        public static Regex ConstPattern = new Regex(@"<c>(?<display>.+?)</c>");

        public static string ExtractContent(this XPathNavigator node)
        {
            if (node == null) return null;

            return XmlTextHelper.NormalizeIndentation(
                ConstPattern.Replace(
                    ParamPattern.Replace(node.InnerXml, GetParamRefName),
                    GetConstRefName)
            );
        }

        public static string GetConstRefName(Match match)
        {
            if (match.Groups.Count != 2) return null;

            return match.Groups["display"].Value;
        }

        public static string GetParamRefName(Match match)
        {
            if (match.Groups.Count != 5) return null;

            return "{" + match.Groups["display"].Value + "}";
        }
    }
}
