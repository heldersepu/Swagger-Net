using System;

namespace Swagger.Net
{
    public interface ISwaggerProvider
    {
        SwaggerDocument GetSwagger(string rootUrl, string apiVersion);
    }

    public class UnknownApiVersion : Exception
    {
        public UnknownApiVersion(string apiVersion)
            : base(String.Format("Unknown API version - {0}", apiVersion))
        {}
    }
}