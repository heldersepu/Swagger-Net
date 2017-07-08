using System;

namespace Swagger.Net.Swagger.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerResponseRemoveDefaultsAttribute : Attribute
    {
    }
}