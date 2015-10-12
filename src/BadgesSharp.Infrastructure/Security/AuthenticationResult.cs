namespace BadgesSharp.Infrastructure.Security
{
    /// <summary>
    /// Represents an authentication result.
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="AuthenticationResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; internal set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; internal set; }
    }
}
