using System;

namespace Swagger.Net.Annotations
{
    /// <summary>
    /// Allows you to add an example value for a parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerExampleAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="example">Example value for a parameter</param>
        public SwaggerExampleAttribute(string parameterName, object example)
        {
            this.ParameterName = parameterName;
            this.Example = example;
        }

        /// <summary>
        /// Parameter name
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Example value for a parameter
        /// </summary>
        public object Example { get; set; }
    }
}