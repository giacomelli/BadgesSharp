using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using BadgesSharp.Infrastructure.Web.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WebApi.OutputCache.Core.Cache;
using WebApi.OutputCache.V2;

namespace BadgesSharp.WebApi
{
    /// <summary>
    /// The web api config.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            var cacheConfig = config.CacheOutputConfiguration();
            cacheConfig.RegisterCacheOutputProvider(() => new MemoryCacheDefault());

            var filters = GlobalConfiguration.Configuration.Filters;
            filters.Add(new WebApiErrorHandlingFilterAttribute());

            var formatters = GlobalConfiguration.Configuration.Formatters;
            var json = formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // A resposta será em JSON quando as chamadas tiverem a extensão .json.
            json.AddUriPathExtensionMapping("json", "application/json");

            // Configura JSON como a resposta padrão.
            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Permite que as enumerações seja serializadas como string.
            json.SerializerSettings.Converters.Add(new StringEnumConverter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
