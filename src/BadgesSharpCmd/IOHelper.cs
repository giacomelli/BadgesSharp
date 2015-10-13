using System;
using System.IO;
using System.Linq;

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
        /// <param name="subfolders">The sub folders.</param>
        /// <param name="currentSubfolderIndex">Index of the current sub folder.</param>
        /// <returns>The folder found or null if not found.</returns>
        public static string GetFirstFolder(string firstFolder, string[] subfolders, int currentSubfolderIndex)
        {
            var currentPattern = subfolders[currentSubfolderIndex];

            if (!firstFolder.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                firstFolder += Path.DirectorySeparatorChar;
            }

            var currentPath = Path.Combine(firstFolder, string.Join(Path.DirectorySeparatorChar.ToString(), subfolders, 1, currentSubfolderIndex - 1));
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
                subfolders[lastIndex] = currentFolderParts[lastIndex];
            }

            currentSubfolderIndex++;

            if (currentFolder != null && currentSubfolderIndex < subfolders.Length)
            {
                return GetFirstFolder(firstFolder, subfolders, currentSubfolderIndex);
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
