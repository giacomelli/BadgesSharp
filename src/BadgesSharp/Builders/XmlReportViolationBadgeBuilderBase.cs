using System.Linq;
using System.Xml.Linq;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// A base class to badge builders that use a xml report as input.
    /// </summary>
    public abstract class XmlReportViolationBadgeBuilderBase : ReportBadgeBuilderBase
    {
        #region Fields
        private string m_violationElementName;
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReportViolationBadgeBuilderBase"/> class.
        /// </summary>
        /// <param name="badgeName">Name of the badge.</param>
        /// <param name="violationElementName">Name of the violation element.</param>
        /// <param name="xmlReport">The XML report.</param>
        protected XmlReportViolationBadgeBuilderBase(string badgeName, string violationElementName, string xmlReport)
            : base(badgeName, xmlReport)
        {
            m_violationElementName = violationElementName;
            Style.Success.Text = "no violations";
            Style.Failed.Text = "{0} violations";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Performs the build.
        /// </summary>
        protected override void PerformBuild()
        {
            int violations = 0;

            if (!string.IsNullOrEmpty(Report))
            {
                var doc = XDocument.Parse(Report);
                violations = doc.Descendants(m_violationElementName).Count();
            }

            TextArguments.Add(violations);
            Status = violations == 0 ? BadgeStatus.Success : BadgeStatus.Failed;
        }
        #endregion
    }
}