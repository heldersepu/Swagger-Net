using Swagger.Net.Application;
using System;
using System.Web.Http;

namespace Swagger.Net.Tests.Owin
{
    public class MultiSwaggerOwinStartup : OwinStartup
    {
        public MultiSwaggerOwinStartup(params Type[] supportedControllers) : base(supportedControllers)
        {
        }

        protected override void EnableSwagger(HttpConfiguration config)
        {
            base.EnableSwagger(config);

            // configure swagger as well on separate URL
            config
                .EnableSwagger("docs/{apiVersion}/.metadata", c => c.SingleApiVersion("v1", "A title for your API"))
                .EnableSwaggerUi("docs-ui/{*assetPath}");
        }
    }
}