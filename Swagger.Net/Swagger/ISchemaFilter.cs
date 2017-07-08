using System;

namespace Swagger.Net.Swagger
{
    public interface ISchemaFilter
    {
        void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type);
    }
}