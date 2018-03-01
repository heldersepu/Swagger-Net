using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace Swagger.Net.Annotations
{
    public class ApplySwaggerResponseAttributes : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (apiDescription.GetControllerAndActionAttributes<SwaggerResponseRemoveDefaultsAttribute>().Any())
                operation.responses.Clear();

            var responseAttributes = apiDescription
                .GetControllerAndActionAttributes<SwaggerResponseAttribute>()
                .OrderBy(attr => attr.StatusCode);
            foreach (var attr in responseAttributes)
            {
                var statusCode = attr.StatusCode.ToString();

                operation.responses[statusCode] = new Response
                {
                    description = attr.Description ?? InferDescriptionFrom(statusCode),
                    schema = (attr.Type != null) ? schemaRegistry.GetOrRegister(attr.Type, attr.TypeName) : null
                };
                if (attr.MediaType != null && attr.Examples != null)
                {
                    operation.responses[statusCode].examples = new Dictionary<string, object> { { attr.MediaType, attr.Examples } };
                }
            }

            var mediaTypes = responseAttributes
                .Where(x => !string.IsNullOrEmpty(x.MediaType))
                .Select(x => x.MediaType).ToList();
            if (mediaTypes.Count > 0)
            {
                operation.produces = mediaTypes;
            }
        }

        public string InferDescriptionFrom(string statusCode)
        {
            HttpStatusCode enumValue;
            if (Enum.TryParse(statusCode, true, out enumValue))
            {
                return enumValue.ToString();
            }
            return null;
        }
    }
}