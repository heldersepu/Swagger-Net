using Swashbuckle.Swagger;
using System.Web.Http;
using System.Web.Http.Description;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    public class AddDefaultResponse : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var errorSchema = schemaRegistry.GetOrRegister(typeof(HttpError));

            operation.responses.Add("default", new Response
                {
                    description = "Error",
                    schema = errorSchema
                });
        }
    }
}
