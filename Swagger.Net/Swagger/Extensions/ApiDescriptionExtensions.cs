using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Description;

namespace Swagger.Net
{
    public static class ApiDescriptionExtensions
    {
        public static string ControllerName(this ApiDescription apiDescription)
        {
            return apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName;
        }

        public static string FriendlyId(this ApiDescription apiDescription)
        {
            return String.Format("{0}_{1}",
                apiDescription.ControllerName(),
                apiDescription.ActionDescriptor.ActionName);
        }

        public static string FriendlyId2(this ApiDescription apiDescription)
        {
            var parts = (apiDescription.HttpMethod.ToString().ToLower() + "/" +
                         apiDescription.RelativePathSansQueryString())
                        .Split('/');

            var builder = new StringBuilder();
            builder.Append(apiDescription.ControllerName() + "_");
            foreach (var part in parts)
            {
                var trimmed = part.Trim('{', '}');
                builder.AppendFormat("{0}{1}",
                    (part.StartsWith("{") ? "By" : string.Empty),
                    trimmed.ToTitleCase()
                );
            }

            return builder.ToString();
        }

        public static IEnumerable<string> Consumes(this ApiDescription apiDescription)
        {
            return apiDescription.SupportedRequestBodyFormatters
                .SelectMany(formatter => formatter.SupportedMediaTypes.Select(mediaType => mediaType.MediaType))
                .Distinct();
        }

        public static IEnumerable<string> Produces(this ApiDescription apiDescription)
        {
            return apiDescription.SupportedResponseFormatters
                .SelectMany(formatter => formatter.SupportedMediaTypes.Select(mediaType => mediaType.MediaType))
                .Distinct();
        }

        public static string RelativePathSansQueryString(this ApiDescription apiDescription)
        {
            return apiDescription.RelativePath.Split('?').First();
        }

        public static Type ResponseType(this ApiDescription apiDesc)
        {
            // HACK: The ResponseDescription property was introduced in WebApi 5.0 but Swagger.Net supports >= 4.0.
            // The reflection hack below provides support for the ResponseType attribute if the application is running
            // against a version of WebApi that supports it.
            var apiDescType = typeof(ApiDescription);

            var responseDescPropInfo = apiDescType.GetProperty("ResponseDescription");
            if (responseDescPropInfo != null)
            {
                var responseDesc = responseDescPropInfo.GetValue(apiDesc, null);
                if (responseDesc != null)
                {
                    var responseDescType = responseDesc.GetType();

                    var responseTypePropInfo = responseDescType.GetProperty("ResponseType");
                    if (responseTypePropInfo != null)
                    {
                        var responseType = responseTypePropInfo.GetValue(responseDesc, null);
                        if (responseType != null)
                            return (Type)responseType;
                    }
                }
            }

            // Otherwise, it defaults to the declared response type
            return apiDesc.ActionDescriptor.ReturnType;
        }

        public static bool IsObsolete(this ApiDescription apiDescription)
        {
            return apiDescription.ActionDescriptor.GetCustomAttributes<ObsoleteAttribute>().Any();
        }

        public static string ObsoleteMessage(this ApiDescription apiDescription)
        {
            return apiDescription.ActionDescriptor.GetCustomAttributes<ObsoleteAttribute>().FirstOrDefault()?.Message;
        }

        public static IEnumerable<TAttribute> GetControllerAndActionAttributes<TAttribute>(this ApiDescription apiDesc)
            where TAttribute : class
        {
            var controllerAttributes = apiDesc.ActionDescriptor.ControllerDescriptor
                .GetCustomAttributes<TAttribute>();

            var actionAttributes = apiDesc.ActionDescriptor
                .GetCustomAttributes<TAttribute>();

            return controllerAttributes.Concat(actionAttributes);
        }
    }
}