using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgesSharpCmd
{
    /// <summary>
    /// IO helper.
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// Gets the folder.
        /// </summary>
        /// <param name="firstFolder">The first folder.</param>
        /// <param name="subFolders">The sub folders.</param>
        /// <param name="currentSubFolderIndex">Index of the current sub folder.</param>
        /// <returns>The folder found or null if not found.</returns>
        public static string GetFirstFolder(string firstFolder, string[] subFolders, int currentSubFolderIndex)
        {
            var currentPattern = subFolders[currentSubFolderIndex];

            if (!firstFolder.EndsWith("\\"))
            {
                firstFolder += Path.DirectorySeparatorChar;
            }

            var currentPath = Path.Combine(firstFolder, string.Join(Path.DirectorySeparatorChar.ToString(), subFolders, 1, currentSubFolderIndex - 1));
            string currentFolder;

            if (currentPattern.Equals("..", StringComparison.OrdinalIgnoreCase))
            {
                currentFolder = Path.Combine(currentPath, currentPattern);
            }
            else
            {
                currentFolder = Directory.GetDirectories(currentPath, currentPattern).FirstOrDefault();
                var currentFolderParts = currentFolder.Split(Path.DirectorySeparatorChar);
                var lastIndex = currentFolderParts.Length - 1;
                subFolders[lastIndex] = currentFolderParts[lastIndex];
            }

            currentSubFolderIndex++;

            if (currentFolder != null && currentSubFolderIndex < subFolders.Length)
            {
                return GetFirstFolder(firstFolder, subFolders, currentSubFolderIndex);
            }

            return currentFolder;
        }

        /// <summary>
        /// Gets the first file.
        /// </summary>
        /// <param name="fileNameWithWildcards">The file name with wildcards.</param>
        /// <returns>The filename found or null if not found.</returns>
        public static string GetFirstFile(string fileNameWithWildcards)
        {
            var candidateFolder = Path.GetDirectoryName(fileNameWithWildcards);
            var foldersTree = candidateFolder.Split(Path.DirectorySeparatorChar);
            var folder = IOHelper.GetFirstFolder(foldersTree[0], foldersTree, 1);

            var pattern = fileNameWithWildcards.Replace(candidateFolder + "\\", string.Empty);

            return Directory.GetFiles(folder, pattern).FirstOrDefault();
        }
    }
}
