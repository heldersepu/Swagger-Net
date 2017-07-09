using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Swagger.Net
{
    public static class JsonContractExtensions
    {
        private static IEnumerable<string> AmbiguousTypeNames = new[]
            {
                "System.Object",
                "System.Net.Http.HttpRequestMessage",
                "System.Net.Http.HttpResponseMessage",
                "System.Web.Http.IHttpActionResult"
            };

        public static bool IsSelfReferencing(this JsonDictionaryContract dictionaryContract)
        {
            return dictionaryContract.UnderlyingType == dictionaryContract.DictionaryValueType;
        }

        public static bool IsSelfReferencing(this JsonArrayContract arrayContract)
        {
            return arrayContract.UnderlyingType == arrayContract.CollectionItemType;
        }

        public static bool IsAmbiguous(this JsonObjectContract objectContract)
        {
            return AmbiguousTypeNames.Contains(objectContract.UnderlyingType.FullName);
        }
    }
}