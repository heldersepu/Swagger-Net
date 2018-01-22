using System.Linq;
using System.Web.Http.Description;

namespace Swagger.Net.Swagger.Annotations
{
    /// <summary>
    /// Apply a SwaggerExampleOperationFilter
    /// </summary>
    public class SwaggerExampleOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                return;
            }

            var customAttributes = apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerExampleAttribute>();

            foreach (Parameter parameter in operation.parameters)
            {
                SwaggerExampleAttribute customAttribute = customAttributes.FirstOrDefault(p => p.ParameterName == parameter.name);
                if (customAttribute != null)
                {
                    parameter.example = customAttribute.Example;
                }
            }
        }
    }
}