using System.Web.Http.Description;

namespace Swagger.Net.Swagger
{
    public interface IDocumentFilter
    {
        void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer);
    }
}
