namespace BadgesSharp.Builders
{
    /// <summary>
    /// A badge style.
    /// </summary>
    public class BadgeStyle
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeStyle"/> class.
        /// </summary>
        public BadgeStyle()
        {
            Success = new BadgeStatusStyle("success", "#97CA00");
            Warning = new BadgeStatusStyle("warning", "#dfb317");
            Failed = new BadgeStatusStyle("failed", "#e05d44");
        }
        #endregion

        #region Properties                        
        /// <summary>
        /// Gets or sets the success.
        /// </summary>
        /// <value>
        /// The success.
        /// </value>
        public BadgeStatusStyle Success { get; set; }

        /// <summary>
        /// Gets or sets the warning.
        /// </summary>
        /// <value>
        /// The warning.
        /// </value>
        public BadgeStatusStyle Warning { get; set; }

        /// <summary>
        /// Gets or sets the failed.
        /// </summary>
        /// <value>
        /// The failed.
        /// </value>
        public BadgeStatusStyle Failed { get; set; }
        #endregion

        #region Methods 
        /// <summary>
        /// Sets all status text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetAllStatusText(string text)
        {
            Failed.Text = text;
            Warning.Text = text;
            Success.Text = text;
        }

        /// <summary>
        /// Gets the status style.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The status style.</returns>
        public BadgeStatusStyle GetStatusStyle(BadgeStatus status)
        {
            switch (status)
            {
                case BadgeStatus.Success:
                    return Success;

                case BadgeStatus.Warning:
                    return Warning;

                default:
                    return Failed;
            }
        }

        /// <summary>
        /// Gets the status style based on value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="warningMinValue">The warning minimum value.</param>
        /// <param name="successMinValue">The success minimum value.</param>
        /// <returns>The status style.</returns>
        public BadgeStatusStyle GetStatusStyle(float value, float warningMinValue, float successMinValue)
        {
            if (value < warningMinValue)
            {
                return Failed;
            }
            else if (value < successMinValue)
            {
                return Warning;
            }

            return Success;
        }
        #endregion
    }
}
