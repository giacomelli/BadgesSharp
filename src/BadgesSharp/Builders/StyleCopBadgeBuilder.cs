namespace BadgesSharp.Builders
{
    /// <summary>
    /// The StyleCop badge builder.
    /// </summary>
    public class StyleCopBadgeBuilder : XmlReportBadgeBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleCopBadgeBuilder"/> class.
        /// </summary>
        /// <param name="xmlReport">The XML report.</param>
        public StyleCopBadgeBuilder(string xmlReport) : base("StyleCop", "Violation", xmlReport)
        {
        }
    }
}
