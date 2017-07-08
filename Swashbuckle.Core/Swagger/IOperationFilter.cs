using System.Web.Http.Description;

namespace Swagger.Net.Swagger
{
    public interface IOperationFilter
    {
        void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription);
    }
}
