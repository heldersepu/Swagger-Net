using Newtonsoft.Json.Serialization;
using Swagger.Net.Swagger.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Swagger.Net
{
    public static class SchemaExtensions
    {
        public static void AddDefault(this Schema schema, object attribute)
        {
            if (attribute is DefaultValueAttribute defAttrib)
                schema.@default = defAttrib.Value;
        }

        public static void AddPattern(this Schema schema, object attribute)
        {
            if (attribute is RegularExpressionAttribute regex)
                schema.pattern = regex.Pattern;
        }

        public static void AddRange(this Schema schema, object attribute)
        {
            if (attribute is RangeAttribute range)
            {
                schema.minimum = range.Minimum;
                schema.maximum = range.Maximum;
            }
        }

        public static void AddLength(this Schema schema, object attribute)
        {
            if (attribute is StringLengthAttribute length)
            {
                schema.maxLength = length.MaximumLength;
                schema.minLength = length.MinimumLength;
            }
            else
            {
                if (attribute is MaxLengthAttribute maxLength)
                    schema.maxLength = maxLength.Length;

                if (attribute is MinLengthAttribute minLength)
                    schema.minLength = minLength.Length;
            }
        }

        public static Schema WithValidationProperties(this Schema schema, JsonProperty jsonProperty)
        {
            var propInfo = jsonProperty.PropertyInfo();
            var fieldInfo = jsonProperty.FieldInfo();
            if (propInfo == null && fieldInfo == null)
                return schema;

            if (propInfo != null)
            {
                foreach (var attribute in propInfo.GetCustomAttributes(false))
                {
                    schema.AddDefault(attribute);
                    schema.AddPattern(attribute);
                    schema.AddRange(attribute);
                    schema.AddLength(attribute);
                }
            }
            if (fieldInfo != null)
            {
                foreach (var attribute in fieldInfo.GetCustomAttributes(false))
                {
                    schema.AddDefault(attribute);
                    schema.AddPattern(attribute);
                    schema.AddRange(attribute);
                    schema.AddLength(attribute);
                }
            }

            if (!jsonProperty.Writable)
                schema.readOnly = true;

            return schema;
        }

        public static Schema WithDescriptionProperty(this Schema schema, JsonProperty jsonProperty)
        {
            var propInfo = jsonProperty.PropertyInfo();
            if (propInfo == null)
                return schema;

            var attrib = propInfo.GetCustomAttributes(false).OfType<SwaggerDescriptionAttribute>().FirstOrDefault();
            schema.description = attrib?.Description;
            return schema;
        }

        public static void PopulateFrom(this PartialSchema partialSchema, Schema schema)
        {
            if (schema == null) return;

            partialSchema.type = schema.type;
            partialSchema.format = schema.format;
            partialSchema.vendorExtensions = schema.vendorExtensions;

            if (schema.items != null)
            {
                // TODO: Handle jagged primitive array and error on jagged object array
                partialSchema.items = new PartialSchema();
                partialSchema.items.PopulateFrom(schema.items);
            }

            partialSchema.@default = schema.@default;
            partialSchema.maximum = schema.maximum;
            partialSchema.exclusiveMaximum = schema.exclusiveMaximum;
            partialSchema.minimum = schema.minimum;
            partialSchema.exclusiveMinimum = schema.exclusiveMinimum;
            partialSchema.maxLength = schema.maxLength;
            partialSchema.minLength = schema.minLength;
            if (schema.pattern != null)
                partialSchema.pattern = schema.pattern;
            partialSchema.maxItems = schema.maxItems;
            partialSchema.minItems = schema.minItems;
            partialSchema.uniqueItems = schema.uniqueItems;
            partialSchema.@enum = schema.@enum;
            partialSchema.multipleOf = schema.multipleOf;
        }
    }
}
