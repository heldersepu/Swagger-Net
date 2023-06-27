namespace Swagger.Net.Dummy.SwaggerExtensions
{
    public class AddDescription : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, System.Type type)
        {
            schema.description = "Some description";
        }
    }
}
