namespace BadgesSharp.WebApi.Models
{
    /// <summary>
    /// The badge view model.
    /// </summary>
    public class BadgeViewModel : Badge
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public BadgeStatus Status { get; set; }
    }
}