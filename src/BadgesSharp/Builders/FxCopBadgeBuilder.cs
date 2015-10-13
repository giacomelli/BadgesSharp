namespace BadgesSharp.Builders
{
    /// <summary>
    /// The FxCop badge builder.
    /// </summary>
    public class FxCopBadgeBuilder : XmlReportViolationBadgeBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FxCopBadgeBuilder"/> class.
        /// </summary>
        /// <param name="xmlReport">The XML report.</param>
        public FxCopBadgeBuilder(string xmlReport) : base("FxCop", "Issue", xmlReport)
        {
        }
    }
}
