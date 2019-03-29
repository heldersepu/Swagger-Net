using System.Reflection;
using System.Xml.XPath;

namespace Swagger.Net.XmlComments
{
    public class ApplyXmlTypeComments : IModelFilter
    {
        private const string MemberXPath = "/doc/members/member[@name='{0}']";
        private const string SummaryTag = "summary";
        private const string ExampleTag = "example";

        private readonly XPathNavigator navigator;

        public ApplyXmlTypeComments(string filePath)
            : this(new XPathDocument(filePath)) { }

        public ApplyXmlTypeComments(XPathDocument xmlDoc)
        {
            lock (xmlDoc)
            {
                navigator = xmlDoc.CreateNavigator();
            }
        }

        public void Apply(Schema model, ModelFilterContext context)
        {
            var commentId = context.SystemType.GetCommentId();
            var typeNode = navigator.SelectSingleNode(string.Format(MemberXPath, commentId));

            if (typeNode != null)
            {
                var summaryNode = typeNode.SelectSingleNode( SummaryTag );
                if( summaryNode != null )
                    model.description = summaryNode.ExtractContent();

                var exampleNode = typeNode.SelectSingleNode( ExampleTag );
                if( exampleNode != null )
                    model.example = exampleNode.ExtractContent();
            }

            if (model.properties != null)
            {
                ApplyPropertyComments(model, context);
            }
        }

        private void ApplyPropertyComments(Schema model, ModelFilterContext context)
        {
            foreach (var entry in model.properties)
            {
                var jsonProperty = context.JsonObjectContract.Properties[entry.Key];
                if (jsonProperty == null) continue;

                var propertyInfo = jsonProperty.PropertyInfo();
                if (propertyInfo != null)
                {
                    var propCommentId = propertyInfo.GetCommentId();
                    ApplyComments(navigator, entry.Value, propCommentId);
                }
                else
                {
                    var fieldInfo = jsonProperty.FieldInfo();
                    if (fieldInfo != null)
                    {
                        var propCommentId = fieldInfo.GetCommentId();
                        ApplyComments(navigator, entry.Value, propCommentId);
                    }
                }
            }
        }

        private void ApplyComments(XPathNavigator navigator, Schema propertySchema, string commentId)
        {
            var propertyNode = navigator.SelectSingleNode(string.Format(MemberXPath, commentId));
            if (propertyNode == null) return;

            var propSummaryNode = propertyNode.SelectSingleNode(SummaryTag);
            if (propSummaryNode != null)
            {
                propertySchema.description = propSummaryNode.ExtractContent();
            }

            var propExampleNode = propertyNode.SelectSingleNode(ExampleTag);
            if (propExampleNode != null)
            {
                propertySchema.example = propExampleNode.ExtractContent();
            }
        }
    }
}