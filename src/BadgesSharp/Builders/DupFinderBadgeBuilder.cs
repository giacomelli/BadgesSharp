namespace BadgesSharp.Builders
{
    /// <summary>
    /// The DupFinder badge builder.
    /// <see href="https://confluence.jetbrains.com/display/NETCOM/Introducing+dupFinder"/>
    /// </summary>
    public class DupFinderBadgeBuilder : XmlReportBadgeBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DupFinderBadgeBuilder"/> class.
        /// </summary>
        /// <param name="xmlReport">The XML report.</param>
        public DupFinderBadgeBuilder(string xmlReport) : base("DupFinder", "Duplicate", xmlReport)
        {
            Style.Success.Text = "no duplicated";
            Style.Failed.Text = "{0} duplicated";
        }
    }
}
