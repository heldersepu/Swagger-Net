using System.Web.Http.Description;

namespace Swagger.Net.Dummy.SwaggerExtensions
{
    public class UpdateFileDownloadOperations : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId == "FileDownload_GetFile")
            {
                operation.produces = new[] { "application/octet-stream" };
            }
        }
    }
}