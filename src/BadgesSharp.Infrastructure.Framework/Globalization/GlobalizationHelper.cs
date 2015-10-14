using HelperSharp;

namespace BadgesSharp.Infrastructure.Framework.Globalization
{
    /// <summary>
    /// Globalization helpers.
    /// </summary>
    public static class GlobalizationHelper
    {
        /// <summary>
        /// Gets the translated text to current culture.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The translated text.</returns>
        public static string GetText(object key)
        {
            return GetText(key, true);
        }

        /// <summary>
        /// Gets the translated text to current culture.
        /// </summary>
        /// <param name="key">The key.</param>        
        /// <param name="markNotFound">If should mark not found keys with the text [TEXT NOT FOUND].</param>
        /// <returns>The translated text.</returns>
        public static string GetText(object key, bool markNotFound)
        {
            ExceptionHelper.ThrowIfNull("key", key);

            Texts.ResourceManager.IgnoreCase = true;
            var result = Texts.ResourceManager.GetString(key.ToString());

            if (markNotFound && result == null)
            {
                result = "[TEXT NOT FOUND] {0}".With(key);
            }

            return result;
        }

        /// <summary>
        /// Gets the translated text to current culture.
        /// </summary>
        /// <param name="key">The key.</param>        
        /// <param name="fallbackKey">The fallback key when the key is not found.</param>
        /// <returns>The translated text.</returns>
        public static string GetText(object key, object fallbackKey)
        {
            var text = GetText(key, false);

            if (text == null)
            {
                text = GetText(fallbackKey);
            }

            return text;
        }      
    }
}