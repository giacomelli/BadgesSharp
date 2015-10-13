using Newtonsoft.Json;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// A base class to badge builders that use a report as input.
    /// </summary>
    public abstract class ReportBadgeBuilderBase : BadgeBuilderBase
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportBadgeBuilderBase"/> class.
        /// </summary>
        /// <param name="badgeName">Name of the badge.</param>
        /// <param name="report">The report.</param>
        protected ReportBadgeBuilderBase(string badgeName, string report)
            : base(badgeName)
        {
            Report = report;
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <value>
        /// The report.
        /// </value>
        protected string Report { get; private set; }
        #endregion
    }
}