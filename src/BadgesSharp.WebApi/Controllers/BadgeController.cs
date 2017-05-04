using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BadgesSharp.Builders;
using BadgesSharp.Infrastructure.Repositories;
using BadgesSharp.Infrastructure.Security;
using BadgesSharp.WebApi.App_Start;
using BadgesSharp.WebApi.Models;
using Skahal.Infrastructure.Framework.Repositories;
using WebApi.OutputCache.V2;
using System.Linq;

namespace BadgesSharp.WebApi.Controllers
{
    /// <summary>
    /// Badges controller.
    /// </summary>
    public class BadgeController : ApiController
    {
        #region Fields
        private IUnitOfWork m_unitOfWork;
        private BadgeService m_badgeService;
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeController"/> class.
        /// </summary>
        public BadgeController()
        {
			m_unitOfWork = new MemoryUnitOfWork();
      		m_badgeService = new BadgeService(new FirebaseRepository<Badge>(m_unitOfWork), m_unitOfWork);
        }
        #endregion

        #region Actions        
        /// <summary>
        /// Gets the specified badge.
        /// </summary>
        /// <param name="repositoryOwner">The repository owner.</param>
        /// <param name="repositoryName">The name of the repository.</param>
        /// <param name="name">The name of the badge.</param>
        /// <returns>The badge url.</returns>
        [HttpGet]
        [CacheOutput(CacheKeyGenerator = typeof(BadgeCacheKeyGenerator), ServerTimeSpan = int.MaxValue)]
        [Route("badges/{repositoryOwner}/{repositoryName}/{name}")]
        public HttpResponseMessage Get(string repositoryOwner, string repositoryName, string name)
        {
            var badge = m_badgeService.GetBadge(repositoryOwner, repositoryName, name);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(badge.Svg, Encoding.UTF8, "image/svg+xml");

            return response;
        }

        [HttpGet]
        [CacheOutput(ServerTimeSpan = int.MaxValue)]
        [Route("badges/available")]
        public IEnumerable<string> GetAvailableBadges()
        {
            return BuilderService.AvailableBadgesNames;
        }

        /// <summary>
        /// Generates the FxCop badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("badges/FxCop")]
        public async Task FxCop(BadgeViewModel badge)
        {
            await GenerateBadgeWithAuth(badge, new FxCopBadgeBuilder(badge.Content));
        }

        /// <summary>
        /// Generates the StyleCop badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("badges/StyleCop")]
        public async Task StypeCop(BadgeViewModel badge)
        {
            await GenerateBadgeWithAuth(badge, new StyleCopBadgeBuilder(badge.Content));
        }

        /// <summary>
        /// Generates the SpecFlow badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("badges/SpecFlow")]
        public async Task SpecFlow(BadgeViewModel badge)
        {
            await GenerateBadgeWithAuth(badge, new SpecFlowBadgeBuilder(badge.Status));
        }

        /// <summary>
        /// Generates the DupFinder badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("badges/DupFinder")]
        public async Task DupFinder(BadgeViewModel badge)
        {
            await GenerateBadgeWithAuth(badge, new DupFinderBadgeBuilder(badge.Content));
        }

        /// <summary>
        /// Generates the Plato Maintainability badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("badges/PlatoMaintainability")]
        public async Task PlatoMaintainability(BadgeViewModel badge)
        {
            await GenerateBadgeWithAuth(badge, new PlatoMaintainabilityBadgeBuilder(badge.Content));
        }

        /// <summary>
        /// Generates the Code Coverage badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("badges/CodeCoverage")]
        public async Task CodeCoverage(BadgeViewModel badge)
        {
            await GenerateBadgeWithAuth(badge, new CodeCoverageBadgeBuilder(badge.Content));
        }

        /// <summary>
        /// Generates the LOC badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("badges/Loc")]
        public async Task Loc(BadgeViewModel badge)
        {
            await GenerateBadgeWithAuth(badge, new LocBadgeBuilder(badge.Content));
        }

        /// <summary>
        /// Gets the generated badges badge.
        /// </summary>
        /// <returns>The badge.</returns>
        [HttpGet]
        [CacheOutput(ServerTimeSpan = 600)]
        [Route("badges/generated/total")]
        public HttpResponseMessage GetGeneratedBadgesBadge()
        {
            var badge = new Badge()
            {
                Owner = "giacomelli",
                Repository = "BadgesSharp",
                Name = "TotalGeneratedBadges"
            };

            GenerateBadge(badge, new TotalGeneratedBadgesBadgeBuilder(m_badgeService));

            return Get(badge.Owner, badge.Repository, badge.Name);
        }
        #endregion

        #region Helpers      				
        /// <summary>
        /// Generates the badge.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <param name="builder">The builder.</param>
        /// <returns>The task.</returns>        
        private async Task GenerateBadgeWithAuth(BadgeViewModel badge, IBadgeSvgBuilder builder)
        {
            var auth = new GitHubAuthentication(badge.Owner, badge.Repository, badge.AccessToken);
            var authResult = await auth.Authenticate();

            if (authResult.Success)
            {
                GenerateBadge(badge, builder);
            }
            else
            {
                throw new InvalidOperationException(authResult.Message);
            }
        }

        private void GenerateBadge(Badge badge, IBadgeSvgBuilder builder)
        {
            var oldBadge = m_badgeService.GetBadge(badge.Owner, badge.Repository, badge.Name);

            badge.Id = oldBadge.Id;
            badge.Svg = builder.Build();
            m_badgeService.SaveBadge(badge);
            m_unitOfWork.Commit();

            BadgeCacheKeyGenerator.ClearBadgeCache(this, badge);
        }
        #endregion
    }
}
