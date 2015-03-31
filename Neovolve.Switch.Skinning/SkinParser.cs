namespace Neovolve.Switch.Skinning
{
    using System;
    using System.IO;
    using Ionic.Zip;

    /// <summary>
    /// The <see cref="SkinParser"/>
    ///   class is used to parse skin files and return a <see cref="SkinDefinition"/> instance.
    /// </summary>
    public static class SkinParser
    {
        /// <summary>
        /// Loads a skin from the specified path.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="loadOverviewOnly">
        /// If set to <c>true</c> [load overview only].
        /// </param>
        /// <returns>
        /// A <see cref="SkinDefinition"/> instance.
        /// </returns>
        public static SkinDefinition Load(String path, Boolean loadOverviewOnly)
        {
            if (File.Exists(path) == false)
            {
                FileNotFoundException innerException = new FileNotFoundException(null, path);

                throw new SkinLoadException(innerException.Message, innerException);
            }

            // This path should be opened as a zip
            using (ZipFile zip = ZipFile.Read(path))
            {
                return LoadSkin(zip, loadOverviewOnly);
            }
        }

        /// <summary>
        /// Loads the specified stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="loadOverviewOnly">
        /// If set to <c>true</c> [load overview only].
        /// </param>
        /// <returns>
        /// A <see cref="SkinDefinition"/> instance.
        /// </returns>
        public static SkinDefinition Load(Stream stream, Boolean loadOverviewOnly)
        {
            using (ZipFile zip = ZipFile.Read(stream))
            {
                return LoadSkin(zip, loadOverviewOnly);
            }
        }

        /// <summary>
        /// Loads the skin.
        /// </summary>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="loadOverviewOnly">
        /// If set to <c>true</c> [load overview only].
        /// </param>
        /// <returns>
        /// A <see cref="SkinDefinition"/> instance.
        /// </returns>
        private static SkinDefinition LoadSkin(ZipFile zip, Boolean loadOverviewOnly)
        {
            String skinDefinitionData = zip.LoadEntryAsText("skin.ini");
            IniFile configuration = IniFile.Load(skinDefinitionData);

            SkinDefinition definition = new SkinDefinition();

            definition.Load(zip, configuration, loadOverviewOnly);

            return definition;
        }
    }
}