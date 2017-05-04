using System.IO;
using System.Text;
using HelperSharp;
using Newtonsoft.Json;
using Skahal.Infrastructure.Framework.Domain;

namespace BadgesSharp
{
    #region Enums
    /// <summary>
    /// Badge status.
    /// </summary>
    public enum BadgeStatus
    {
        /// <summary>
        /// The badge represents a success status.
        /// </summary>
        Success,

        /// <summary>
        /// The badge represents a warning status.
        /// </summary>
        Warning,

        /// <summary>
        /// The badge represents a failed status.
        /// </summary>
        Failed
    }
    #endregion

    /// <summary>
    /// Represents a badge.
    /// </summary>
    [JsonObject]
	public class Badge : EntityWithIdBase<string>, IAggregateRoot
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgesSharp.Badge"/> class.
        /// </summary>
        public Badge()
        {
            Id = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public string Repository { get; set; }

        /// <summary>
        /// Gets or sets the SVG in base64.
        /// </summary>
        /// <value>
        /// The SVG.
        /// </value>
        public string Svg { get; set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get
            {
                return GetFileName(Name, Owner, Repository);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="badgeName">Name of the badge.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>The badge filename.</returns>
        public static string GetFileName(string badgeName, string owner, string repository)
        {
            return Path.Combine(@"{0}\{1}\{2}.svg".With(owner, repository, badgeName));
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>The bytes of SVG.</returns>
        public byte[] GetData()
        {
            if (string.IsNullOrEmpty(Svg))
            {
                return new byte[0];
            }

            return Encoding.UTF8.GetBytes(Svg);
        }

        #endregion
    }
}
