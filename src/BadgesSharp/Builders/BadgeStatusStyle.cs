namespace BadgesSharp.Builders
{
    /// <summary>
    /// Badge stage style.
    /// </summary>
    public class BadgeStatusStyle
    {
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeStatusStyle"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        public BadgeStatusStyle(string text, string color)
        {
            Text = text;
            Color = color;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; }
        #endregion
    }
}
