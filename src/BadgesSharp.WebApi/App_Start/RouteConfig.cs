using System.Web.Mvc;
using System.Web.Routing;

namespace BadgesSharp.WebApi
{
    /// <summary>
    /// The route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Docs", action = "Index", id = UrlParameter.Optional });
        }
    }
}
