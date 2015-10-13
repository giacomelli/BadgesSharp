using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using HelperSharp;
using WebApi.OutputCache.V2;

namespace BadgesSharp.WebApi.App_Start
{
    /// <summary>
    /// Badge cache key generator.
    /// </summary>
    public class BadgeCacheKeyGenerator : ICacheKeyGenerator
    {
        /// <summary>
        /// Makes the badge cache key.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="name">The name.</param>
        /// <returns>The cache key.</returns>
        public static string MakeBadgeCacheKey(object owner, object repository, object name)
        {
            return "{0}_{1}_{2}".With(owner, repository, name);
        }

        /// <summary>
        /// Clears the badge cache.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="badge">The badge.</param>
        public static void ClearBadgeCache(ApiController controller, Badge badge)
        {
            var cache = controller.Configuration.CacheOutputConfiguration().GetCacheOutputProvider(controller.Request);
            cache.RemoveStartsWith(MakeBadgeCacheKey(badge.Owner, badge.Repository, badge.Name));
            cache.RemoveStartsWith(MakeBadgeCacheKey("giacomelli", "BadgesSharp", "TotalGeneratedBadges"));
        }

        /// <summary>
        /// Makes the cache key.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="excludeQueryString">if set to <c>true</c> [exclude query string].</param>
        /// <returns>The cache key.</returns>
        public string MakeCacheKey(HttpActionContext context, MediaTypeHeaderValue mediaType, bool excludeQueryString = false)
        {
            var args = context.ActionArguments;

            return MakeBadgeCacheKey(args["repositoryOwner"], args["repositoryName"], args["name"]);
        }
    }
}