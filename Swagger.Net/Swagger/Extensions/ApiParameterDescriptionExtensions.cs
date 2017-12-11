using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http.Description;

namespace Swagger.Net.Swagger.Extensions
{
    public static class ApiParameterDescriptionExtensions
    {
        public static RegularExpressionAttribute GetRegularExpressionAttribute(this ApiParameterDescription paramDesc)
        {
            return paramDesc.ParameterDescriptor.GetCustomAttributes<RegularExpressionAttribute>().FirstOrDefault();
        }

        public static DescriptionAttribute GetDescriptionAttribute(this ApiParameterDescription paramDesc)
        {
            return paramDesc.ParameterDescriptor.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
        }
    }
}
