namespace BadgesSharp.Builders
{
    /// <summary>
    /// The SpecFlow badge builder.
    /// </summary>
    public class SpecFlowBadgeBuilder : BadgeBuilderBase
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
