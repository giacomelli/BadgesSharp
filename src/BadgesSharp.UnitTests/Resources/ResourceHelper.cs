using System;
using System.IO;
using TestSharp;

namespace BadgesSharp.UnitTests
{
    public static class ResourceHelper
    {
        public static string GetResource(string filename)
        {
            var fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", filename);

            return File.ReadAllText(fullpath);
        }
    }
}
