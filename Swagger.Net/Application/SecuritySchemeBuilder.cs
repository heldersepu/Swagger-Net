using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Swagger.Net.Application
{
    public abstract class SecuritySchemeBuilder
    {
        internal abstract SecurityScheme Build();
    }

    public class BasicAuthSchemeBuilder : SecuritySchemeBuilder
    {
        private string _description;

        public BasicAuthSchemeBuilder Description(string description)
        {
            _description = description;
            return this;
        }

        internal override SecurityScheme Build()
        {
            return new SecurityScheme
            {
                type = "basic",
                description = _description
            };
        }
    }

    public class ApiKeySchemeBuilder : SecuritySchemeBuilder
    {
        public string description;
        public string name;
        public string @in;
        public Type type;

        public ApiKeySchemeBuilder(string name, string @in, string description, Type type)
        {
            this.description = description;
            this.name = name;
            this.@in = @in;
            this.type = type;
        }

        internal override SecurityScheme Build()
        {
            return new SecurityScheme
            {
                type = "apiKey",
                description = description,
                name = name,
                @in = @in
            };
        }
    }

    public class OAuth2SchemeBuilder : SecuritySchemeBuilder
    {
        private string _description;
        private string _flow;
        private string _authorizationUrl;
        private string _tokenUrl;
        private IDictionary<string, string> _scopes = new Dictionary<string, string>();

        public OAuth2SchemeBuilder Description(string description)
        {
            _description = description;
            return this;
        }

        public OAuth2SchemeBuilder Flow(string flow)
        {
            _flow = flow;
            return this;
        }

        public OAuth2SchemeBuilder AuthorizationUrl(string authorizationUrl)
        {
            _authorizationUrl = authorizationUrl;
            return this;
        }

        public OAuth2SchemeBuilder TokenUrl(string tokenUrl)
        {
            _tokenUrl = tokenUrl;
            return this;
        }

        public OAuth2SchemeBuilder Scopes(Action<IDictionary<string, string>> configure)
        {
            configure(_scopes);
            return this;
        }

        internal override SecurityScheme Build()
        {
            // TODO: Validate required fields for given flow

            return new SecurityScheme
            {
                type = "oauth2",
                flow = _flow,
                authorizationUrl = _authorizationUrl,
                tokenUrl = _tokenUrl,
                scopes = _scopes,
                description = _description,
            };
        }
    }
}