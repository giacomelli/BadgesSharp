using System.ComponentModel;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// The SpecFlow badge builder.
    /// </summary>
    [Browsable(false)] // Need to find a report or something to be the input to SpecFlow badge.
    public sealed class SpecFlowBadgeBuilder : BadgeBuilderBase
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecFlowBadgeBuilder"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        public SpecFlowBadgeBuilder(BadgeStatus status) : base("SpecFlow")
        {
            Status = status;
        }
        #endregion
    }
}
