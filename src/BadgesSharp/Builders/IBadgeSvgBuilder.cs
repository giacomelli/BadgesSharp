namespace BadgesSharp.Builders
{
    /// <summary>
    /// Defines an interface to a badge SVG builder.
    /// </summary>
    public interface IBadgeSvgBuilder
    {
        /// <summary>
        /// Builds the SVG.
        /// </summary>
        /// <returns>The SVG badge content.</returns>
        string Build();
    }
}
