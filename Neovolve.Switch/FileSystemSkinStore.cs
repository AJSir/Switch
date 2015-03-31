namespace Neovolve.Switch
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Neovolve.Switch.Skinning;

    /// <summary>
    /// The <see cref="FileSystemSkinStore"/>
    ///   class is used to manage a skin store using a file system directory.
    /// </summary>
    public class FileSystemSkinStore : ISkinStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemSkinStore"/> class.
        /// </summary>
        /// <param name="directory">
        /// The directory.
        /// </param>
        public FileSystemSkinStore(String directory)
        {
            Contract.Requires<ArgumentNullException>(
                String.IsNullOrWhiteSpace(directory) == false, "The directory value is null, empty or only contains whitespace.");

            BasePath = ResolvePath(directory);

            EnsurePathExists();
        }

        /// <summary>
        /// Resolves the skins.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable&lt;T&gt;"/> instance.
        /// </returns>
        public IEnumerable<Skin> ResolveSkins()
        {
            DirectoryInfo directory = new DirectoryInfo(BasePath);

            if (directory.Exists == false)
            {
                yield break;
            }

            IEnumerable<Skin> zipSkins = SearchForSkins(directory, "*.zip");

            foreach (Skin skin in zipSkins)
            {
                yield return skin;
            }

            IEnumerable<Skin> szfSkins = SearchForSkins(directory, "*.szf");

            foreach (Skin skin in szfSkins)
            {
                yield return skin;
            }
        }

        /// <summary>
        /// Stores the skin.
        /// </summary>
        /// <param name="skinPath">
        /// The skin path.
        /// </param>
        /// <returns>
        /// The new path of the skin.
        /// </returns>
        public String StoreSkin(String skinPath)
        {
            String fileName = Path.GetFileName(skinPath);
            String destinationPath = Path.Combine(BasePath, fileName);

            try
            {
                // Check that the skin can be loaded
                SkinDefinition definition = SkinParser.Load(skinPath, true);

                if (definition == null)
                {
                    return null;
                }

                File.Copy(skinPath, destinationPath);

                return destinationPath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Resolves the path.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String ResolvePath(String path)
        {
            String resolvedPath = Environment.ExpandEnvironmentVariables(path);

            if (Path.IsPathRooted(resolvedPath))
            {
                return resolvedPath;
            }

            // Make this path relative to the application directory
            String assemblyLocation = typeof(FileSystemSkinStore).Assembly.Location;
            String assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            if (assemblyDirectory == null)
            {
                return resolvedPath;
            }

            if (Directory.Exists(assemblyDirectory))
            {
                // Combine the paths
                resolvedPath = Path.Combine(assemblyDirectory, resolvedPath);
            }

            return resolvedPath;
        }

        /// <summary>
        /// Searches for skins.
        /// </summary>
        /// <param name="directory">
        /// The directory.
        /// </param>
        /// <param name="searchPattern">
        /// The search pattern.
        /// </param>
        /// <returns>
        /// A <see cref="IEnumerable&lt;T&gt;"/> instance.
        /// </returns>
        private static IEnumerable<Skin> SearchForSkins(DirectoryInfo directory, String searchPattern)
        {
            IEnumerable<FileInfo> zipFiles = directory.EnumerateFiles(searchPattern, SearchOption.AllDirectories);

            foreach (FileInfo zipFile in zipFiles)
            {
                Skin zipSkin;

                try
                {
                    zipSkin = new Skin(zipFile.FullName);
                }
                catch (SkinLoadException)
                {
                    continue;
                }

                yield return zipSkin;
            }
        }

        /// <summary>
        /// Ensures the path exists.
        /// </summary>
        private void EnsurePathExists()
        {
            if (String.IsNullOrWhiteSpace(BasePath))
            {
                // No path is available so we will skip the creation
                return;
            }

            if (Directory.Exists(BasePath))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(BasePath);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            catch (IOException)
            {
                return;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is writable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is writable; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWritable
        {
            get
            {
                // Create a temporary file name
                String tempFileName = Guid.NewGuid().ToString("N") + ".tmp";
                String tempFilePath = Path.Combine(BasePath, tempFileName);

                try
                {
                    using (FileStream fileStream = File.Open(tempFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    {
                        fileStream.WriteByte(32);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return false;
                }
                catch (IOException)
                {
                    return false;
                }
                finally
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch (IOException)
                    {
                        // Ignore this exception
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>
        /// The base path.
        /// </value>
        private String BasePath
        {
            get;
            set;
        }
    }
}