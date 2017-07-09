using System;

namespace Swagger.Net.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerResponseRemoveDefaultsAttribute : Attribute
    {
    }
}