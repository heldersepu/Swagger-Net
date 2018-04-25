using Newtonsoft.Json;
using Swagger.Net.Annotations;
using Swagger.Net.FromUriParams;
using Swagger.Net.XmlComments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace Swagger.Net.Application
{
    public class SwaggerDocsConfig
    {
        private VersionInfoBuilder _versionInfoBuilder;
        private Func<ApiDescription, string, bool> _versionSupportResolver;
        private IEnumerable<string> _schemes;
        private IDictionary<string, SecuritySchemeBuilder> _securitySchemeBuilders;
        private bool _prettyPrint;
        private bool _ignoreObsoleteActions;
        private string _accessControlAllowOrigin;
        private bool _cachingSwaggerDoc;
        private Func<ApiDescription, string> _groupingKeySelector;
        private IComparer<string> _groupingKeyComparer;
        private readonly IDictionary<Type, Func<Schema>> _customSchemaMappings;
        private readonly IList<Func<ISchemaFilter>> _schemaFilters;
        private readonly IList<Func<IModelFilter>> _modelFilters;
        private Func<Type, string> _schemaIdSelector;
        private bool _ignoreObsoleteProperties;
        private bool _describeAllEnumsAsStrings;
        private bool _describeStringEnumsInCamelCase;
        private bool _ignoreObsoleteEnumConstants;
        private bool _applyFiltersToAllSchemas = true;
        private bool _ignoreIsSpecifiedMembers = false;
        private readonly IList<Func<IOperationFilter>> _operationFilters;
        private readonly IList<Func<IDocumentFilter>> _documentFilters;
        private readonly IList<Func<XPathDocument>> _xmlDocFactories;
        private Func<IEnumerable<ApiDescription>, ApiDescription> _conflictingActionsResolver;
        private Func<HttpRequestMessage, string> _rootUrlResolver;
        private Func<ApiDescription, string> _operationIdResolver = null;

        private Func<ISwaggerProvider, ISwaggerProvider> _customProviderFactory;

        public SwaggerDocsConfig()
        {
            _versionInfoBuilder = new VersionInfoBuilder();
            _securitySchemeBuilders = new Dictionary<string, SecuritySchemeBuilder>();
            _prettyPrint = false;
            _ignoreObsoleteActions = false;
            _accessControlAllowOrigin = null;
            _cachingSwaggerDoc = false;
            _customSchemaMappings = new Dictionary<Type, Func<Schema>>();
            _schemaFilters = new List<Func<ISchemaFilter>>();
            _modelFilters = new List<Func<IModelFilter>>();
            _ignoreObsoleteProperties = false;
            _describeAllEnumsAsStrings = false;
            _describeStringEnumsInCamelCase = false;
            _ignoreObsoleteEnumConstants = false;
            _operationFilters = new List<Func<IOperationFilter>>();
            _documentFilters = new List<Func<IDocumentFilter>>();
            _xmlDocFactories = new List<Func<XPathDocument>>();
            _rootUrlResolver = DefaultRootUrlResolver;

            SchemaFilter<ApplySwaggerSchemaFilterAttributes>();

            //TODO: Can we improve performance by consolidating the OperationFilters?
            OperationFilter<HandleFromUriParams>();
            OperationFilter<ApplySwaggerOperationAttributes>();
            OperationFilter<ApplySwaggerResponseAttributes>();
            OperationFilter<ApplySwaggerOperationFilterAttributes>();
            OperationFilter<ApplySwaggerExampleAttribute>();
        }

        public InfoBuilder SingleApiVersion(string version, string title)
        {
            _versionSupportResolver = null;
            _versionInfoBuilder = new VersionInfoBuilder();
            return _versionInfoBuilder.Version(version, title);
        }

        public void MultipleApiVersions(
            Func<ApiDescription, string, bool> versionSupportResolver,
            Action<VersionInfoBuilder> configure)
        {
            _versionSupportResolver = versionSupportResolver;
            _versionInfoBuilder = new VersionInfoBuilder();
            configure(_versionInfoBuilder);
        }

        public void Schemes(IEnumerable<string> schemes)
        {
            _schemes = schemes;
        }

        public BasicAuthSchemeBuilder BasicAuth(string name)
        {
            var schemeBuilder = new BasicAuthSchemeBuilder();
            _securitySchemeBuilders[name] = schemeBuilder;
            return schemeBuilder;
        }

        public ApiKeySchemeBuilder ApiKey(string name, string @in, string description, Type type = null)
        {
            if (type == null) type = typeof(AuthorizeAttribute);
            var apiKeyScheme = new ApiKeySchemeBuilder(name, @in, description, type);
            _securitySchemeBuilders[name] = apiKeyScheme;
            return apiKeyScheme;
        }

        public OAuth2SchemeBuilder OAuth2(string name)
        {
            var schemeBuilder = new OAuth2SchemeBuilder();
            _securitySchemeBuilders[name] = schemeBuilder;
            return schemeBuilder;
        }

        public void PrettyPrint()
        {
            _prettyPrint = true;
        }

        public void IgnoreObsoleteActions()
        {
            _ignoreObsoleteActions = true;
        }

        public void AccessControlAllowOrigin(string value)
        {
            _accessControlAllowOrigin = value;
        }

        public void AllowCachingSwaggerDoc()
        {
            _cachingSwaggerDoc = true;
        }

        public void GroupActionsBy(Func<ApiDescription, string> keySelector)
        {
            _groupingKeySelector = keySelector;
        }

        public void OrderActionGroupsBy(IComparer<string> keyComparer)
        {
            _groupingKeyComparer = keyComparer;
        }

        public void MapType<T>(Func<Schema> factory)
        {
            MapType(typeof(T), factory);
        }

        public void MapType(Type type, Func<Schema> factory)
        {
            _customSchemaMappings.Add(type, factory);
        }

        public void SchemaFilter<TFilter>()
            where TFilter : ISchemaFilter, new()
        {
            SchemaFilter(() => new TFilter());
        }

        public void SchemaFilter(Func<ISchemaFilter> factory)
        {
            _schemaFilters.Add(factory);
        }

        // NOTE: In next major version, ModelFilter will completely replace SchemaFilter
        public void ModelFilter<TFilter>() where TFilter : IModelFilter, new()
        {
            ModelFilter(() => new TFilter());
        }

        // NOTE: In next major version, ModelFilter will completely replace SchemaFilter
        public void ModelFilter(Func<IModelFilter> factory)
        {
            _modelFilters.Add(factory);
        }

        public void SchemaId(Func<Type, string> schemaIdStrategy)
        {
            _schemaIdSelector = schemaIdStrategy;
        }

        public void UseFullTypeNameInSchemaIds()
        {
            _schemaIdSelector = t => t.FriendlyId(true);
        }

        public void DescribeAllEnumsAsStrings(bool camelCase = false)
        {
            _describeAllEnumsAsStrings = true;
            _describeStringEnumsInCamelCase = camelCase;
        }

        public void IgnoreObsoleteEnumConstants()
        {
            _ignoreObsoleteEnumConstants = true;
        }

        public void IgnoreIsSpecifiedMembers()
        {
            _ignoreIsSpecifiedMembers = true;
        }

        public void IgnoreObsoleteProperties()
        {
            _ignoreObsoleteProperties = true;
        }

        public void OperationFilter<TFilter>()
            where TFilter : IOperationFilter, new()
        {
            OperationFilter(() => new TFilter());
        }

        public void OperationFilter(Func<IOperationFilter> factory)
        {
            _operationFilters.Add(factory);
        }

        public void DocumentFilter<TFilter>()
            where TFilter : IDocumentFilter, new()
        {
            DocumentFilter(() => new TFilter());
        }

        public void DocumentFilter(Func<IDocumentFilter> factory)
        {
            _documentFilters.Add(factory);
        }

        public void IncludeXmlComments(Func<XPathDocument> xmlDocFactory)
        {
            _xmlDocFactories.Add(xmlDocFactory);
        }

        public void IncludeXmlComments(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var xPath = new XPathDocument(filePath);
                    _xmlDocFactories.Add(() => xPath);
                }
                catch {}
            }
            else
                throw new FileNotFoundException("XML Comment file not found!");
        }

        public void IncludeXmlComments(string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                IncludeXmlComments(filePath);
            }
        }

        public void IncludeAllXmlComments(Assembly assembly, string directory)
        {
            if (assembly != null)
            {
                foreach (var name in assembly.GetManifestResourceNames().Where(x => x.ToUpper().EndsWith(".XML")))
                {
                    try
                    {
                        var xPath = new XPathDocument(assembly.GetManifestResourceStream(name));
                        IncludeXmlComments(() => xPath);
                    }
                    catch {}
                }
            }

            if (!string.IsNullOrEmpty(directory))
            {
                foreach (var name in Directory.GetFiles(directory, "*.XML", SearchOption.AllDirectories))
                {
                    IncludeXmlComments(filePath: name);
                }
            }
        }

        public void ResolveConflictingActions(Func<IEnumerable<ApiDescription>, ApiDescription> conflictingActionsResolver)
        {
            _conflictingActionsResolver = conflictingActionsResolver;
        }

        public void OperationIdResolver(Func<ApiDescription, string> operationIdResolver)
        {
            _operationIdResolver = operationIdResolver;
        }

        public void RootUrl(Func<HttpRequestMessage, string> rootUrlResolver)
        {
            _rootUrlResolver = rootUrlResolver;
        }

        public void CustomProvider(Func<ISwaggerProvider, ISwaggerProvider> customProviderFactory)
        {
            _customProviderFactory = customProviderFactory;
        }

        public ISwaggerProvider GetSwaggerProvider(HttpRequestMessage swaggerRequest)
        {
            var httpConfig = swaggerRequest.GetConfiguration();

            var securityDefintitions = _securitySchemeBuilders.Any()
                ? _securitySchemeBuilders.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Build())
                : null;

            // NOTE: Instantiate & add the XML comments filters here so they're executed before any
            // custom filters AND so they can share the same XPathDocument (perf. optimization)
            var modelFilters = _modelFilters.Select(factory => factory()).ToList();
            var operationFilters = _operationFilters.Select(factory => factory()).ToList();
            foreach (var xmlDocFactory in _xmlDocFactories)
            {
                var xmlDoc = xmlDocFactory();
                modelFilters.Insert(0, new ApplyXmlTypeComments(xmlDoc));
                operationFilters.Insert(0, new ApplyXmlActionComments(xmlDoc));
            }

            foreach (var s in _securitySchemeBuilders.Where(x => x.Value.GetType() == typeof(ApiKeySchemeBuilder)))
            {
                var apiKeySch = (ApiKeySchemeBuilder)s.Value;
                operationFilters.Add(new ApplySwaggerOperationApiKey(apiKeySch.name, apiKeySch.type));
            }

            var options = new SwaggerGeneratorOptions(
                versionSupportResolver: _versionSupportResolver,
                schemes: _schemes,
                securityDefinitions: securityDefintitions,
                ignoreObsoleteActions: _ignoreObsoleteActions,
                accessControlAllowOrigin: _accessControlAllowOrigin,
                groupingKeySelector: _groupingKeySelector,
                groupingKeyComparer: _groupingKeyComparer,
                customSchemaMappings: _customSchemaMappings,
                schemaFilters: _schemaFilters.Select(factory => factory()).ToList(),
                modelFilters: modelFilters,
                ignoreObsoleteProperties: _ignoreObsoleteProperties,
                schemaIdSelector: _schemaIdSelector,
                describeAllEnumsAsStrings: _describeAllEnumsAsStrings,
                describeStringEnumsInCamelCase: _describeStringEnumsInCamelCase,
                ignoreObsoleteEnumConstants: _ignoreObsoleteEnumConstants,
                applyFiltersToAllSchemas: _applyFiltersToAllSchemas,
                ignoreIsSpecifiedMembers: _ignoreIsSpecifiedMembers,
                operationFilters: operationFilters,
                documentFilters: _documentFilters.Select(factory => factory()).ToList(),
                conflictingActionsResolver: _conflictingActionsResolver,
                operationIdResolver: _operationIdResolver
            );

            var defaultProvider = new SwaggerGenerator(
                httpConfig.Services.GetApiExplorer(),
                httpConfig.SerializerSettingsOrDefault(),
                _versionInfoBuilder.Build(),
                options);

            return (_customProviderFactory != null)
                ? _customProviderFactory(defaultProvider)
                : defaultProvider;
        }

        internal string GetRootUrl(HttpRequestMessage swaggerRequest)
        {
            return _rootUrlResolver(swaggerRequest);
        }

        internal IEnumerable<string> DiscoveryPaths(string route)
        {
            return GetApiVersions().Select(version => route.Replace("{apiVersion}", version));
        }

        internal string GetAccessControlAllowOrigin()
        {
            return _accessControlAllowOrigin;
        }

        internal bool NoCachingSwaggerDoc()
        {
            return !_cachingSwaggerDoc;
        }

        internal IEnumerable<string> GetApiVersions()
        {
            return _versionInfoBuilder.Build().Select(entry => entry.Key);
        }

        internal Formatting GetFormatting()
        {
            return _prettyPrint ? Formatting.Indented : Formatting.None;
        }

        public static string DefaultRootUrlResolver(HttpRequestMessage request)
        {
            var scheme = GetHeaderValue(request, "X-Forwarded-Proto") ?? request.RequestUri.Scheme;
            var host = GetHeaderValue(request, "X-Forwarded-Host") ?? request.RequestUri.Host;
            var port = GetHeaderValue(request, "X-Forwarded-Port") ?? request.RequestUri.Port.ToString(CultureInfo.InvariantCulture);
            var prefix = GetHeaderValue(request, "X-Forwarded-Prefix") ?? string.Empty;

            var httpConfiguration = request.GetConfiguration();
            var virtualPathRoot = httpConfiguration.VirtualPathRoot;
            var urb = new UriBuilder(scheme, host, int.Parse(port), prefix + virtualPathRoot);
            return urb.Uri.AbsoluteUri.TrimEnd('/');
        }

        private static string GetHeaderValue(HttpRequestMessage request, string headerName)
        {
            IEnumerable<string> list;
            return request.Headers.TryGetValues(headerName, out list) ? list.FirstOrDefault() : null;
        }
    }
}