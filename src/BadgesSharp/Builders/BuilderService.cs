using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HelperSharp;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// The builder service.
    /// </summary>
    public static class BuilderService
    {
        #region Fields
        private static IList<Type> s_availableBuilders;
        #endregion

        #region Constructors                
        /// <summary>
        /// Initializes static members of the <see cref="BuilderService"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Discover the badge builders.")]
        static BuilderService()
        {
            s_availableBuilders = ReflectionHelper.GetSubclassesOf<BadgeBuilderBase>();
            InternalAvailableBadgesNames = s_availableBuilders.Select(b => b.Name.Replace("BadgeBuilder", string.Empty)).ToList().AsReadOnly();
            AvailableBadgesNames = InternalAvailableBadgesNames.Where(
                b =>
                   !b.Equals("SpecFlow", StringComparison.OrdinalIgnoreCase)
                && !b.Equals("TotalGeneratedBadges", StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the internal available badges names.
        /// </summary>
        /// <value>
        /// The available badges names.
        /// </value>
        private static IList<string> InternalAvailableBadgesNames { get; set; }

        /// <summary>
        /// Gets the internal available badges names.
        /// </summary>
        /// <value>
        /// The available badges names.
        /// </value>
        public static IList<string> AvailableBadgesNames { get; private set; }
        #endregion

        #region Methods                
        /// <summary>
        /// Checks whether exists a builder for the specified badge name.
        /// </summary>
        /// <param name="badgeName">Name of the badge.</param>
        /// <returns>True if exists a builder.</returns>
        public static bool ExistsBuilder(string badgeName)
        {
            return InternalAvailableBadgesNames.Any(b => b.Equals(badgeName, StringComparison.OrdinalIgnoreCase));
        }
        #endregion
    }
}
