using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Swagger.Net
{
    public static class JsonPropertyExtensions
    {
        public static bool IsRequired(this JsonProperty jsonProperty)
        {
            return jsonProperty.HasAttribute<RequiredAttribute>() || jsonProperty.Required == Required.Always;
        }

        public static bool IsObsolete(this JsonProperty jsonProperty)
        {
            return jsonProperty.HasAttribute<ObsoleteAttribute>();
        }

        public static bool HasAttribute<T>(this JsonProperty jsonProperty)
        {
            var propInfo = jsonProperty.PropertyInfo();
            if (propInfo != null)
            {
                return Attribute.IsDefined(propInfo, typeof(T));
            }
            else
            {
                var fieldInfo = jsonProperty.FieldInfo();
                return fieldInfo != null && Attribute.IsDefined(fieldInfo, typeof(T)); ;
            }
        }

        public static PropertyInfo PropertyInfo(this JsonProperty jsonProperty)
        {
            if (jsonProperty.UnderlyingName == null) return null;

            var metadata = jsonProperty.DeclaringType.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                .FirstOrDefault();

            var typeToReflect = (metadata != null)
                ? ((MetadataTypeAttribute)metadata).MetadataClassType
                : jsonProperty.DeclaringType;

            return typeToReflect.GetProperty(jsonProperty.UnderlyingName, jsonProperty.PropertyType);
        }

        public static FieldInfo FieldInfo(this JsonProperty jsonProperty)
        {
            if (jsonProperty.UnderlyingName == null) return null;

            var metadata = jsonProperty.DeclaringType.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                .FirstOrDefault();

            var typeToReflect = (metadata != null)
                ? ((MetadataTypeAttribute)metadata).MetadataClassType
                : jsonProperty.DeclaringType;

            return typeToReflect.GetField(jsonProperty.UnderlyingName);
        }
    }
}