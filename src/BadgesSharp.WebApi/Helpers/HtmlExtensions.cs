using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BadgesSharp.Infrastructure.Framework.Globalization;

namespace BadgesSharp.WebApi.Helpers
{
    /// <summary>
    /// Html helper extensions.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Translate the text.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="key">The key.</param>
        /// <returns>The translated text</returns>
        public static string Translate(this string key)
        {
            var translated = GlobalizationHelper.GetText(key, false);

            if(String.IsNullOrEmpty(translated))
            {
                translated = key;
            }

            return translated;
        }
    }
}