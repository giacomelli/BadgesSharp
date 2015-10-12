using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgesSharp.Infrastructure
{
    /// <summary>
    /// A configuration helper.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// The BadgesSharp API url.
        /// </summary>
#if RELEASE
        public const string WebApiUrl = "https://badgessharp.apphb.com";
#else
        public const string WebApiUrl = "http://localhost:8888";
#endif

        /// <summary>
        /// Gets the GData client email.
        /// </summary>
        /// <value>
        /// The g data client email.
        /// </value>
        public static string GDataClientEmail
        {
            get
            {
                return Get("GDataClientEmail");
            }
        }

        /// <summary>
        /// Gets the GData private key.
        /// </summary>
        /// <value>
        /// The g data private key.
        /// </value>
        public static string GDataPrivateKey
        {
            get
            {
                return Get("GDataPrivateKeyPart1") + Get("GDataPrivateKeyPart2");
            }
        }

		/// <summary>
		/// Gets the Parse application id.
		/// </summary>
		/// <value>The id.</value>
		public static string ParseApplicationId
		{
			get
			{
				return Get("ParseApplicationId");
			}
		}

		/// <summary>
		/// Gets the Pars .NET key.
		/// </summary>
		/// <value>The key.</value>
		public static string ParseDotNetKey
		{
			get
			{
				return Get("ParseDotNetKey");
			}
		}

        /// <summary>
        /// Gets the GitHub access token.
        /// </summary>
        /// <value>
        /// The git hub access token.
        /// </value>
        public static string GitHubAccessToken
        {
            get
            {
                return Get("GitHubAccessToken");
            }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static string Configuration
        {
            get
            {
                return Get("Configuration");
            }
        }

        /// <summary>
        /// Gets the specified value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        private static string Get(string key)
        {
			var appSettigs = ConfigurationManager.AppSettings;
            var fromConfig = appSettigs[key];

			if (string.Equals(fromConfig, "{ENV}", StringComparison.OrdinalIgnoreCase) || !appSettigs.AllKeys.Contains(key))
            {
                return Environment.GetEnvironmentVariable("BadgesSharp_" + key);
            }
            else
            {
                return fromConfig;
            }
        }
    }
}
