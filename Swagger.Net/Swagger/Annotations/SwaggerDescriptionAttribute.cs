using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swagger.Net.Swagger.Annotations
{
    public class SwaggerDescriptionAttribute: Attribute
    {
        public SwaggerDescriptionAttribute(string description = null, string summary = null)
        {
            Description = description;
            Summary = summary;
        }

        public string Description { get; set; }
        public string Summary { get; set; }
    }
}
