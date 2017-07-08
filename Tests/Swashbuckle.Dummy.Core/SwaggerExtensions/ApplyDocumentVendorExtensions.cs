using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    public class ApplyDocumentVendorExtensions : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.vendorExtensions.Add("x-document", "foo");
        }
    }
}
