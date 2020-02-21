using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Swagger.Net
{
    public class SchemaRegistry
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly SwaggerGeneratorOptions _options;
        private readonly IContractResolver _contractResolver;
        private readonly ICollection<WorkItem> _workItems;

        private class WorkItem
        {
            public string SchemaId;
            public Type Type;
            public bool InProgress;
            public bool Done;
            public Schema Schema;
        }

        public SchemaRegistry(JsonSerializerSettings jsonSerializerSettings, SwaggerGeneratorOptions options)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            _options = options;

            _contractResolver = jsonSerializerSettings.ContractResolver ?? new DefaultContractResolver
            {
                IgnoreIsSpecifiedMembers = options.IgnoreIsSpecifiedMembers
            };
            _workItems = new List<WorkItem>();
            Definitions = new Dictionary<string, Schema>();
        }

        public Schema GetOrRegister(Type type, string typeName = null)
        {
            var schema = CreateInlineSchema(type, typeName);

            var workItemToBeProcessed = _workItems.FirstOrDefault(workItem => !workItem.Done && !workItem.InProgress);

            // Iterate outstanding work items (i.e. referenced types) and generate the corresponding definition
            while (workItemToBeProcessed != null)
            {
                workItemToBeProcessed.InProgress = true;
                workItemToBeProcessed.Schema = CreateDefinitionSchema(workItemToBeProcessed.Type);
                Definitions.Add(workItemToBeProcessed.SchemaId, workItemToBeProcessed.Schema);
                workItemToBeProcessed.InProgress = false;
                workItemToBeProcessed.Done = true;

                workItemToBeProcessed = _workItems.FirstOrDefault(workItem => !workItem.Done && !workItem.InProgress);
            }

            return schema;
        }

        public IDictionary<string, Schema> Definitions { get; private set; }

        public Schema CreateInlineSchema(Type type, string typeName = null)
        {
            var jsonContract = _contractResolver.ResolveContract(type);

            if (_options.CustomSchemaMappings.ContainsKey(type))
                return FilterSchema(_options.CustomSchemaMappings[type](), jsonContract);

            if (jsonContract is JsonPrimitiveContract)
                return FilterSchema(CreatePrimitiveSchema((JsonPrimitiveContract)jsonContract), jsonContract);

            var dictionaryContract = jsonContract as JsonDictionaryContract;
            if (dictionaryContract != null)
                return dictionaryContract.IsSelfReferencing()
                    ? CreateRefSchema(type, typeName)
                    : FilterSchema(CreateDictionarySchema(dictionaryContract), jsonContract);

            var arrayContract = jsonContract as JsonArrayContract;
            if (arrayContract != null)
                return arrayContract.IsSelfReferencing()
                    ? CreateRefSchema(type, typeName)
                    : FilterSchema(CreateArraySchema(arrayContract, true, typeName), jsonContract);

            var objectContract = jsonContract as JsonObjectContract;
            if (objectContract != null && !objectContract.IsAmbiguous())
                return CreateRefSchema(type, typeName);

            // Fallback to abstract "object"
            return FilterSchema(new Schema { type = "object" }, jsonContract);
        }

        public Schema CreateDefinitionSchema(Type type)
        {
            var jsonContract = _contractResolver.ResolveContract(type);

            return CreateDefinitionSchema(jsonContract);
        }

        public Schema CreateDefinitionSchema(JsonContract jsonContract)
        {
            if (jsonContract is JsonDictionaryContract)
                return FilterSchema(CreateDictionarySchema((JsonDictionaryContract)jsonContract), jsonContract);

            if (jsonContract is JsonArrayContract)
                return FilterSchema(CreateArraySchema((JsonArrayContract)jsonContract, false), jsonContract);

            if (jsonContract is JsonObjectContract)
                return FilterSchema(CreateObjectSchema((JsonObjectContract)jsonContract, true), jsonContract);

            throw new InvalidOperationException("Unsupported type for Definitions. Must be Dictionary, Array or Object");
        }

        public Schema CreatePrimitiveSchema(JsonPrimitiveContract primitiveContract)
        {
            var type = Nullable.GetUnderlyingType(primitiveContract.UnderlyingType) ?? primitiveContract.UnderlyingType;

            if (type.IsEnum)
                return CreateEnumSchema(primitiveContract, type);

            switch (type.FullName)
            {
                case "System.Boolean":
                    return new Schema { type = "boolean" };
                case "System.Byte":
                case "System.SByte":
                case "System.Int16":
                case "System.UInt16":
                case "System.Int32":
                case "System.UInt32":
                    return new Schema { type = "integer", format = "int32" };
                case "System.Int64":
                case "System.UInt64":
                    return new Schema { type = "integer", format = "int64" };
                case "System.Single":
                    return new Schema { type = "number", format = "float" };
                case "System.Double":
                case "System.Decimal":
                    return new Schema { type = "number", format = "double" };
                case "System.Byte[]":
                    return new Schema { type = "string", format = "byte" };
                case "System.DateTime":
                case "System.DateTimeOffset":
                    return new Schema { type = "string", format = "date-time" };
                case "System.Guid":
                    return new Schema { type = "string", format = "uuid", example = Guid.Empty };
                default:
                    return new Schema { type = "string" };
            }
        }

        public Schema CreateEnumSchema(JsonPrimitiveContract primitiveContract, Type type)
        {
            var stringEnumConverter = primitiveContract.Converter as StringEnumConverter
                ?? _jsonSerializerSettings.Converters.OfType<StringEnumConverter>().FirstOrDefault();

            if (_options.DescribeAllEnumsAsStrings || stringEnumConverter != null)
            {
                var camelCase = _options.DescribeStringEnumsInCamelCase
                    || (stringEnumConverter != null && stringEnumConverter.CamelCaseText);

                var enumValues = type.GetEnumNamesForSerialization(_options.IgnoreObsoleteEnumConstants);

                return new Schema
                {
                    type = "string",
                    @enum = camelCase
                        ? enumValues.Select(name => name.ToCamelCase()).ToArray()
                        : enumValues
                };
            }

            return new Schema
            {
                type = "integer",
                format = "int32",
                @enum = type.GetEnumValuesForSerialization(_options.IgnoreObsoleteEnumConstants)
            };
        }

        public Schema CreateDictionarySchema(JsonDictionaryContract dictionaryContract)
        {
            var keyType = dictionaryContract.DictionaryKeyType ?? typeof(object);
            var valueType = dictionaryContract.DictionaryValueType ?? typeof(object);

            if (keyType.IsEnum)
            {
                return new Schema
                {
                    type = "object",
                    properties = Enum.GetNames(keyType).ToDictionary(
                        (name) => dictionaryContract.DictionaryKeyResolver(name),
                        (name) => CreateInlineSchema(valueType)
                    )
                };
            }
            else
            {
                return new Schema
                {
                    type = "object",
                    additionalProperties = CreateInlineSchema(valueType)
                };
            }
        }

        public Schema CreateArraySchema(JsonArrayContract arrayContract, bool isWrapped = false, string typeName = null)
        {
            var itemType = arrayContract.CollectionItemType ?? typeof(object);
            var s = new Schema
            {
                type = "array",
                items = CreateInlineSchema(itemType)
            };
            if (arrayContract.CreatedType.Name.StartsWith("HashSet"))
            {
                s.uniqueItems = true;
            }
            if (itemType.Namespace != "System" && itemType.Namespace != "Enum")
            {
                s.xml = new Xml { name = typeName ?? itemType.Name, wrapped = isWrapped };
            }
            return s;
        }

        public Schema CreateObjectSchema(JsonObjectContract jsonContract, bool addXmlName = false)
        {
            var properties = jsonContract.Properties
                .Where(p => !p.Ignored)
                .Where(p => !(_options.IgnoreObsoleteProperties && p.IsObsolete()))
                .ToDictionary(
                    prop => prop.PropertyName,
                    prop => CreateInlineSchema(prop.PropertyType)
                        .WithValidationProperties(prop)
                        .WithDescriptionProperty(prop)
                );

            var required = jsonContract.Properties.Where(prop => prop.IsRequired())
                .Select(propInfo => propInfo.PropertyName)
                .ToList();

            var s = new Schema
            {
                required = required.Any() ? required : null,
                properties = properties,
                type = "object"
            };
            if (addXmlName)
            {
                s.xml = new Xml { name = jsonContract.UnderlyingType.Name };
            }
            return s;
        }

        private Schema CreateRefSchema(Type type, string typeName)
        {
            var schemaId = typeName ?? _options.SchemaIdSelector(type);

            var existingWorkItem = _workItems.FirstOrDefault(entry => entry.SchemaId == schemaId);

            if (existingWorkItem == null)
            {
                _workItems.Add(new WorkItem { SchemaId = schemaId, Type = type });
            }
            else
            {
                if (existingWorkItem.Type != type)
                {
                    throw new InvalidOperationException(String.Format(
                        "Conflicting schemaIds: Duplicate schemaIds detected for types {0} and {1}. " +
                        "See the config setting - \"UseFullTypeNameInSchemaIds\" for a potential workaround",
                        type.FullName, existingWorkItem.Type.FullName));
                }
            }

            return new Schema { @ref = "#/definitions/" + schemaId };
        }

        private Schema FilterSchema(Schema schema, JsonContract jsonContract)
        {
            if (schema.type == "object" || _options.ApplyFiltersToAllSchemas)
            {
                var jsonObjectContract = jsonContract as JsonObjectContract;
                if (jsonObjectContract != null)
                {
                    // NOTE: In next major version, _modelFilters will completely replace _schemaFilters
                    var modelFilterContext = new ModelFilterContext(jsonObjectContract.UnderlyingType, jsonObjectContract, this);
                    foreach (var filter in _options.ModelFilters)
                    {
                        filter.Apply(schema, modelFilterContext);
                    }
                }

                foreach (var filter in _options.SchemaFilters)
                {
                    filter.Apply(schema, this, jsonContract.UnderlyingType);
                }
            }

            return schema;
        }
    }
}