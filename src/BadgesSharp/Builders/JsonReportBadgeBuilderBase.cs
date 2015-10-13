using Newtonsoft.Json;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// A base class to badge builders that use a json report as input.
    /// </summary>
    public abstract class JsonReportBadgeBuilderBase : ReportBadgeBuilderBase
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonReportBadgeBuilderBase"/> class.
        /// </summary>
        /// <param name="badgeName">Name of the badge.</param>
        /// <param name="jsonReport">The JSON report.</param>
        protected JsonReportBadgeBuilderBase(string badgeName, string jsonReport)
            : base(badgeName, jsonReport)
        {
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Reads the json.
        /// </summary>
        /// <returns>The JSON object.</returns>
        protected dynamic ReadJson()
        {
            return JsonConvert.DeserializeObject<dynamic>(Report);
        }
        #endregion
    }
}