using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// The Code Coverage badge builder.
    /// </summary>
    [Browsable(false)] // Need to document how to use it in TeamCity.
    public class CodeCoverageBadgeBuilder : ReportBadgeBuilderBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCoverageBadgeBuilder"/> class.
        /// </summary>
        /// <param name="xmlReport">The XML report.</param>
        public CodeCoverageBadgeBuilder(string xmlReport) : base("Code coverage",  xmlReport)
        {
            Style.SetAllStatusText("{0:N0}%");
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Performs the build.
        /// </summary>
        protected override void PerformBuild()
        {
            var doc = new XmlDocument();
            doc.LoadXml(Report);

            try
            {
                var coveragePercent = Convert.ToSingle(doc["Root"].Attributes["CoveragePercent"].Value, CultureInfo.InvariantCulture);
                TextArguments.Add(coveragePercent);
                this.SetStatusByPercent(coveragePercent);
            }
            catch (NullReferenceException ex)
            {
                throw new InvalidOperationException("Code coverage report file format unsupported. The supported formats are: DotCover", ex);
            }
        }
        #endregion
    }
}
