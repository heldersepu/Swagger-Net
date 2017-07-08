using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    public class SupportFlaggedEnums : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null) return;

            var queryEnumParams = operation.parameters
                .Where(param => param.@in == "query" && param.@enum != null)
                .ToArray();

            foreach (var param in queryEnumParams)
            {
                param.items = new PartialSchema { type = param.type, @enum = param.@enum };
                param.type = "array";
                param.collectionFormat = "csv";
            }
        }
    }
}