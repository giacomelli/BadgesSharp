using System;
using BadgesSharp.Builders;
using DotBadge;
using HelperSharp;
using Skahal.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Repositories;

namespace BadgesSharp
{
    /// <summary>
    /// Badge service.
    /// </summary>
    public class BadgeService : ServiceBase<Badge, IRepository<Badge>, IUnitOfWork>
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public BadgeService(IRepository<Badge> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Gets the badge.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="name">The name.</param>
        /// <returns>The badge.</returns>
        public Badge GetBadge(string owner, string repository, string name)
        {
            var badge = MainRepository.FindFirst(b => b.Owner == owner && b.Repository == repository && b.Name == name);

            // No data yet? 
            if (badge == null)
            {
                if (BuilderService.ExistsBuilder(name))
                {
                    badge = CreateDefaultBadge(owner, repository, name);
                }
                else
                {
                    throw new InvalidOperationException("There is no builder for badge '{0}'. The available badges are: {1}".With(name, string.Join(", ", BuilderService.AvailableBadgesNames)));
                }
            }

            return badge;
        }

        /// <summary>
        /// Generates the badge.
        /// </summary>
        /// <param name="badge">The information.</param>
        public void SaveBadge(Badge badge)
        {
            MainRepository[badge.Id] = badge;
        }

        /// <summary>
        /// Counts the badges.
        /// </summary>
        /// <returns>The total badges.</returns>
        public long CountBadges()
        {
            return MainRepository.CountAll();
        }
        #endregion

        #region Private methods                     
        /// <summary>
        /// Creates the default badge.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="name">The name.</param>
        /// <returns>The default badge.</returns>
        private static Badge CreateDefaultBadge(string owner, string repository, string name)
        {
            var badge = new Badge()
            {
                Owner = owner,
                Repository = repository,
                Name = name
            };

            var painter = new BadgePainter();
            badge.Svg = painter.DrawSVG(name, "no data", "#c7c713");

            return badge;
        }
        #endregion
    }
}
