using System;
using System.Collections.Generic;
using System.Linq;

namespace Swagger.Net.Annotations
{
    public class ApplySwaggerSchemaFilterAttributes : ISchemaFilter
    {
        private IEnumerable<SwaggerSchemaFilterAttribute> Attributes(Type type)
        {
            var attributes = type.GetCustomAttributes(true).OfType<SwaggerSchemaFilterAttribute>();
            if (attributes.Count() == 0 && type.BaseType != null)
                return Attributes(type.BaseType);
            else
                return attributes;
        }

        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            foreach (var attribute in Attributes(type))
            {
                var filter = (ISchemaFilter)Activator.CreateInstance(attribute.FilterType);
                filter.Apply(schema, schemaRegistry, type);
            }
        }
    }
}
