using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;

namespace Swagger.Net.Annotations
{
    public class ApplySwaggerOperationApiKey : IOperationFilter
    {
        private string _apiKey;
        private Type _type;

        public ApplySwaggerOperationApiKey(string apiKey, Type type)
        {
            _apiKey = apiKey;
            _type = type;
        }

        public void Apply(Operation o, SchemaRegistry s, ApiDescription a)
        {
            var scopes = a.ActionDescriptor.GetFilterPipeline()
                        .Select(filterInfo => filterInfo.Instance)
                        .Where(x =>_type.IsInstanceOfType(x))
                        .Distinct();

            if (scopes.Any())
            {
                if (o.security == null)
                    o.security = new List<IDictionary<string, IEnumerable<string>>>();
                else if (o.security.Any(x => x.ContainsKey(_apiKey)))
                    return;

                var SecRequirements = new Dictionary<string, IEnumerable<string>>
                    {
                        { _apiKey, new List<string>() }
                    };
                o.security.Add(SecRequirements);
            }
        }
    }
}
