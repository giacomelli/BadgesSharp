using System;
using System.Globalization;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// The Plato Maintainability badge builder.
    /// </summary>
    public class PlatoMaintainabilityBadgeBuilder : JsonReportBadgeBuilderBase
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatoMaintainabilityBadgeBuilder"/> class.
        /// </summary>
        /// <param name="jsonReport">The json report.</param>
        public PlatoMaintainabilityBadgeBuilder(string jsonReport) : base("Plato", jsonReport)
        {
            Style.SetAllStatusText("Maintainability: {0:N0}%");
        }
        #endregion

        #region Methods  
        /// <summary>
        /// Performs the build.
        /// </summary>
        protected override void PerformBuild()
        {
            float maintainability = 0;
            var json = ReadJson();

            if (json != null)
            {
                maintainability = Convert.ToSingle(json.summary.average.maintainability, CultureInfo.InvariantCulture);
            }

            TextArguments.Add(maintainability);
            this.SetStatusByPercent(maintainability);           
        }
        #endregion
    }
}
