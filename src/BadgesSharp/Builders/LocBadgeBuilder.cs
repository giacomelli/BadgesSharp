using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using HelperSharp;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// The LOC (Lines Of Code) badge builder.
    /// </summary>
    [Browsable(false)] // Need to document how to use SourceMonitor in command line.
    public class LocBadgeBuilder : ReportBadgeBuilderBase
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="LocBadgeBuilder"/> class.
        /// </summary>
        /// <param name="xmlReport">The json report.</param>
        public LocBadgeBuilder(string xmlReport) : base("LOC", xmlReport)
        {
            Style.SetAllStatusText("{0}");
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
                var linesMetric = doc.SelectSingleNode("//metric_name[text()='Lines']");
                var metricXpath = "//metric[@id='{0}']".With(linesMetric.Attributes["id"].Value);
                var loc = Convert.ToInt32(doc.SelectSingleNode(metricXpath).InnerText, CultureInfo.InvariantCulture);
                TextArguments.Add(loc);
                Status = BadgeStatus.Success;
            }
            catch (NullReferenceException ex)
            {
                throw new InvalidOperationException("LOC report file format unsupported. The supported formats are: SourceMonitor", ex);
            }
        }
        #endregion
    }
}
