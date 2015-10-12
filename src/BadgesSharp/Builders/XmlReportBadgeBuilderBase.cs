using System.Linq;
using System.Xml.Linq;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// A base class to badge builders that use a xml report as input.
    /// </summary>
    public abstract class XmlReportBadgeBuilderBase : BadgeBuilderBase
    {
        #region Fields
        private string m_violationElementName;
        private string m_xmlReport;
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReportBadgeBuilderBase"/> class.
        /// </summary>
        /// <param name="badgeName">Name of the badge.</param>
        /// <param name="violationElementName">Name of the violation element.</param>
        /// <param name="xmlReport">The XML report.</param>
        protected XmlReportBadgeBuilderBase(string badgeName, string violationElementName, string xmlReport)
            : base(badgeName)
        {
            m_violationElementName = violationElementName;
            m_xmlReport = xmlReport;
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

            if (!string.IsNullOrEmpty(m_xmlReport))
            {
                var doc = XDocument.Parse(m_xmlReport);
                violations = doc.Descendants(m_violationElementName).Count();
            }

            TextArguments.Add(violations);
            Status = violations == 0 ? BadgeStatus.Success : BadgeStatus.Failed;
        }
        #endregion
    }
}