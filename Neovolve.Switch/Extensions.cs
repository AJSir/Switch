namespace Neovolve.Switch
{
    using System;
    using System.Configuration;
    using Neovolve.Switch.Properties;

    /// <summary>
    /// The <see cref="Extensions"/>
    ///   class is used to provide common extension methods.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Copies the settings.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="destination">
        /// The destination.
        /// </param>
        public static void CopySettings(this Settings source, Settings destination)
        {
            foreach (Object property in source.Properties)
            {
                SettingsProperty setting = property as SettingsProperty;

                if (setting == null)
                {
                    continue;
                }

                destination[setting.Name] = source[setting.Name];
            }
        }
    }
}