using System.Collections.Generic;
using System.Linq;
using BadgesSharp.Resources;
using DotBadge;
using HelperSharp;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// A base class to badge SVG builders.
    /// </summary>
    public abstract class BadgeBuilderBase : IBadgeSvgBuilder
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeBuilderBase"/> class.
        /// </summary>
        /// <param name="badgeName">Name of the badge.</param>
        protected BadgeBuilderBase(string badgeName)
        {
            BadgeName = badgeName;
            Style = new BadgeStyle();
            Status = BadgeStatus.Failed;
            TextArguments = new List<object>();
        }
        #endregion

        #region Properties   
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public BadgeStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        protected BadgeStyle Style { get; set; }

        /// <summary>
        /// Gets the name of the badge.
        /// </summary>
        /// <value>
        /// The name of the badge.
        /// </value>
        protected string BadgeName
        {
            get; private set;
        }

        /// <summary>
        /// Gets the text arguments.
        /// </summary>
        /// <value>
        /// The text arguments.
        /// </value>
        protected IList<object> TextArguments { get; private set; }
        #endregion

        #region Methods        
        /// <summary>
        /// Builds the SVG.
        /// </summary>
        /// <returns>
        /// The SVG badge content.
        /// </returns>
        public string Build()
        {
            PerformBuild();
            var statusStyle = Style.GetStatusStyle(Status);
            var text = statusStyle.Text.With(TextArguments.ToArray());

            var painter = new BadgePainter();
            return painter.DrawSVG(BadgeName, text, statusStyle.Color, DotBadge.Style.Custom, Templates.Default);
        }

        /// <summary>
        /// Performs the build.
        /// </summary>
        protected virtual void PerformBuild()
        {
        }
        #endregion
    }
}